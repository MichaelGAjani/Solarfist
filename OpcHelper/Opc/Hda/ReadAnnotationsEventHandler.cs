namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;
    using System.Runtime.CompilerServices;

    public delegate void ReadAnnotationsEventHandler(IRequest request, AnnotationValueCollection[] results);
}

