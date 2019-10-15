// FileInfo
// File:"QRHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:QR
// 1.CreateSVGQR(string code, int size)
// 2.CreateQR(string code, int size)
//
// File Lines:46
using System.Drawing;
using ThoughtWorks.QRCode.Codec;

namespace Jund.NETHelper.MultiMediaHelper
{
    public class QRHelper
    {
        public string CreateSVGQR(string code, int size)
        {
            QRCodeCore.QRCodeData qr = new QRCodeCore.QRCodeData(code);
            QRCodeCore.SvgQRCode svg = new QRCodeCore.SvgQRCode(qr);
            return svg.Create(size);
        }

        public Image CreateQR(string code, int size)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            //设置编码模式
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            //设置编码测量度
            qrCodeEncoder.QRCodeScale = size;
            //设置编码版本
            qrCodeEncoder.QRCodeVersion = 8;
            //设置编码错误纠正
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            return qrCodeEncoder.Encode(code);
        }
    }
}
