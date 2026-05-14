using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace BioManager.Services;

public class QrCodeService
{
    public byte[] GenerateQrCode(string data)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(20); 
    }
}