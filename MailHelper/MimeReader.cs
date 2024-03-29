﻿namespace Jund.MailHelper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Mime;

    public class MimeReader
    {
        private MimeEntity _entity;
        private Queue<string> _lines;
        private static readonly char[] HeaderWhitespaceChars = new char[] { ' ', '\t' };

        private MimeReader()
        {
            this._entity = new MimeEntity();
        }

        public MimeReader(string[] lines) : this()
        {
            if (lines == null)
            {
                throw new ArgumentNullException("lines");
            }
            this._lines = new Queue<string>(lines);
        }

        private MimeReader(MimeEntity entity, Queue<string> lines) : this()
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            if (lines == null)
            {
                throw new ArgumentNullException("lines");
            }
            this._lines = lines;
            this._entity = new MimeEntity(entity);
        }

        private void AddChildEntity(MimeEntity entity, Queue<string> lines)
        {
            MimeReader reader = new MimeReader(entity, lines);
            entity.Children.Add(reader.CreateMimeEntity());
        }

        public MimeEntity CreateMimeEntity()
        {
            try
            {
                this.ParseHeaders();
                this.ProcessHeaders();
                this.ParseBody();
                this.SetDecodedContentStream();
                return this._entity;
            }
            catch
            {
                return null;
            }
        }

        private byte[] GetBytes(string content)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(content);
                }
                return stream.ToArray();
            }
        }

        public static ContentType GetContentType(string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
            {
                contentType = "text/plain; charset=us-ascii";
            }
            return new ContentType(contentType);
        }

        public static string GetMediaMainType(string mediaType)
        {
            int index = mediaType.IndexOf('/');
            if (index < 0)
            {
                return mediaType;
            }
            return mediaType.Substring(0, index);
        }

        public static string GetMediaSubType(string mediaType)
        {
            int index = mediaType.IndexOf('/');
            if (index < 0)
            {
                if (mediaType.Equals("text"))
                {
                    return "plain";
                }
                return string.Empty;
            }
            if (mediaType.Length > index)
            {
                return mediaType.Substring(index + 1);
            }
            if (GetMediaMainType(mediaType).Equals("text"))
            {
                return "plain";
            }
            return string.Empty;
        }

        public static string GetMediaType(string mediaType)
        {
            if (string.IsNullOrEmpty(mediaType))
            {
                return "text/plain";
            }
            return mediaType.Trim();
        }

        public static TransferEncoding GetTransferEncoding(string transferEncoding)
        {
            switch (transferEncoding.Trim().ToLowerInvariant())
            {
                case "7bit":
                case "8bit":
                    return TransferEncoding.SevenBit;

                case "quoted-printable":
                    return TransferEncoding.QuotedPrintable;

                case "base64":
                    return TransferEncoding.Base64;
            }
            return TransferEncoding.Unknown;
        }

        private void ParseBody()
        {
            if (!this._entity.HasBoundary)
            {
                while (this._lines.Count > 0)
                {
                    this._entity.EncodedMessage.Append(this._lines.Dequeue() + "\r\n");
                }
            }
            else
            {
                while ((this._lines.Count > 0) && !string.Equals(this._lines.Peek(), this._entity.EndBoundary))
                {
                    if ((this._entity.Parent != null) && string.Equals(this._entity.Parent.StartBoundary, this._lines.Peek()))
                    {
                        break;
                    }
                    if (string.Equals(this._lines.Peek(), this._entity.StartBoundary))
                    {
                        this.AddChildEntity(this._entity, this._lines);
                    }
                    else
                    {
                        if (string.Equals(this._entity.ContentType.MediaType, MediaTypes.MessageRfc822, StringComparison.InvariantCultureIgnoreCase) && string.Equals(this._entity.ContentDisposition.DispositionType, "attachment", StringComparison.InvariantCultureIgnoreCase))
                        {
                            this.AddChildEntity(this._entity, this._lines);
                            break;
                        }
                        this._entity.EncodedMessage.Append(this._lines.Dequeue() + "\r\n");
                    }
                }
            }
        }

        private int ParseHeaders()
        {
            string str = string.Empty;
            string category = string.Empty;
            while ((this._lines.Count > 0) && !string.IsNullOrEmpty(this._lines.Peek()))
            {
                category = this._lines.Dequeue();
                if (category.StartsWith(" ") || category.StartsWith(Convert.ToString('\t')))
                {
                    this._entity.Headers[str] = this._entity.Headers[str] + category;
                }
                else
                {
                    int index = category.IndexOf(':');
                    if (index < 0)
                    {
                        Debug.WriteLine("Invalid header:{0}", category);
                        continue;
                    }
                    string str3 = category.Substring(0, index);
                    string str4 = category.Substring(index + 1).Trim(HeaderWhitespaceChars);
                    this._entity.Headers.Add(str3.ToLower(), str4);
                    str = str3;
                }
            }
            if (this._lines.Count > 0)
            {
                this._lines.Dequeue();
            }
            return this._entity.Headers.Count;
        }

        private void ProcessHeaders()
        {
            foreach (string str in this._entity.Headers.AllKeys)
            {
                string str2 = str;
                if (str2 != null)
                {
                    if (str2 != "content-description")
                    {
                        if (str2 == "content-disposition")
                        {
                            goto Label_00A9;
                        }
                        if (str2 == "content-id")
                        {
                            goto Label_00D0;
                        }
                        if (str2 == "content-transfer-encoding")
                        {
                            goto Label_00F2;
                        }
                        if (str2 == "content-type")
                        {
                            goto Label_0133;
                        }
                        if (str2 == "mime-version")
                        {
                            goto Label_0157;
                        }
                    }
                    else
                    {
                        this._entity.ContentDescription = this._entity.Headers[str];
                    }
                }
                continue;
            Label_00A9:
                this._entity.ContentDisposition = new ContentDisposition(this._entity.Headers[str]);
                continue;
            Label_00D0:
                this._entity.ContentId = this._entity.Headers[str];
                continue;
            Label_00F2:
                this._entity.TransferEncoding = this._entity.Headers[str];
                this._entity.ContentTransferEncoding = GetTransferEncoding(this._entity.Headers[str]);
                continue;
            Label_0133:
                this._entity.SetContentType(GetContentType(this._entity.Headers[str]));
                continue;
            Label_0157:
                this._entity.MimeVersion = this._entity.Headers[str];
            }
        }

        private void SetDecodedContentStream()
        {
            switch (this._entity.ContentTransferEncoding)
            {
                case TransferEncoding.QuotedPrintable:
                    this._entity.Content = new MemoryStream(this.GetBytes(QuotedPrintableEncoding.Decode(this._entity.EncodedMessage.ToString())), false);
                    return;

                case TransferEncoding.Base64:
                    this._entity.Content = new MemoryStream(Convert.FromBase64String(this._entity.EncodedMessage.ToString()), false);
                    return;
            }
            this._entity.Content = new MemoryStream(this.GetBytes(this._entity.EncodedMessage.ToString()), false);
        }

        public Queue<string> Lines =>
            this._lines;
    }
}

