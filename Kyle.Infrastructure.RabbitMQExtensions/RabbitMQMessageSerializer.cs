using System.Text;

namespace Kyle.Infrastructure.RabbitMQExtensions;

public class RabbitMQMessageSerializer
{
    public string ToBase64String(byte[] bytes)
    {
        return Convert.ToBase64String(bytes);
    }

    public byte[] FromBase64String(string bytes)
    {
        return Convert.FromBase64String(bytes);
    }

    private static readonly byte[] EmptyBytes = new byte[0];

    public byte[] SerializeObject(RabbitMQMessage message)
    {
        var messageCodeBytes = BitConverter.GetBytes(message.Code);

        var messageCreatedTimeTicksBytes = BitConverter.GetBytes(message.CreatedTime.Ticks);

        var topicBytes = Encoding.UTF8.GetBytes(message.Topic);
        var topicLengthBytes = BitConverter.GetBytes(topicBytes.Length);

        var tagBytes = EmptyBytes;
        if (!string.IsNullOrWhiteSpace(message.Tag))
        {
            tagBytes = Encoding.UTF8.GetBytes(message.Tag);
        }
        var tagLengthBytes = BitConverter.GetBytes(tagBytes.Length);

        return ByteUtil.Combine(messageCodeBytes, messageCreatedTimeTicksBytes,
            topicLengthBytes, topicBytes,
            tagLengthBytes, tagBytes, message.Body);
    }

    public RabbitMQMessage DeserializeObject(byte[] messageBuffer)
    {
        var messageCodeBytes = new byte[4];
        var messageCreatedTimeTicksBytes = new byte[8];
        var topicLengthBytes = new byte[4];
        var tagLengthBytes = new byte[4];
        var srcOffset = 0;
        
        //messageCode
        Buffer.BlockCopy(messageBuffer,srcOffset,messageCodeBytes,0,4);
        srcOffset += 4;
        
        Buffer.BlockCopy(messageBuffer,srcOffset,messageCreatedTimeTicksBytes,0,8);
        srcOffset += 8;
        
        Buffer.BlockCopy(messageBuffer,srcOffset,topicLengthBytes,0,4);
        srcOffset += 4;

        var topicLength = BitConverter.ToInt32(topicLengthBytes, 0);
        var topicBytes = new byte[topicLength];
        Buffer.BlockCopy(messageBuffer,srcOffset,topicBytes,0,topicLength);
        srcOffset += topicLength;
        
        Buffer.BlockCopy(messageBuffer,srcOffset,tagLengthBytes,0,4);
        srcOffset += 4;
        var tagLength = BitConverter.ToInt32(tagLengthBytes, 0);
        var tagBytes = new byte[tagLength];
        Buffer.BlockCopy(messageBuffer,srcOffset,tagBytes,0,tagLength);
        srcOffset += tagLength;

        var bodyBytes = new byte[messageBuffer.Length - srcOffset];
        Buffer.BlockCopy(messageBuffer,srcOffset,bodyBytes,0,bodyBytes.Length);
        var code = BitConverter.ToInt32(messageCodeBytes, 0);
        var createdTimeTicks = BitConverter.ToInt64(messageCreatedTimeTicksBytes, 0);
        var createTime = new DateTime(createdTimeTicks);
        var topic = Encoding.UTF8.GetString(topicBytes);
        var tag = Encoding.UTF8.GetString(tagBytes);

        return new RabbitMQMessage(topic, code, bodyBytes, createTime, tag);

    }
}

public class ByteUtil
{
    public static readonly byte[] ZeroLengthBytes = BitConverter.GetBytes(0);
    public static readonly byte[] EmptyBytes = new byte[0];

    public static void EncodeString(string data, out byte[] lengthBytes, out byte[] dataBytes)
    {
        if (data != null)
        {
            dataBytes = Encoding.UTF8.GetBytes(data);
            lengthBytes = BitConverter.GetBytes(dataBytes.Length);
        }
        else
        {
            dataBytes = EmptyBytes;
            lengthBytes = ZeroLengthBytes;
        }
    }

    public static byte[] EncodeDateTime(DateTime data)
    {
        return BitConverter.GetBytes(data.Ticks);
    }

    public static string DecodeString(byte[] sourceBuffer, int startOffset, out int nextStartOffset)
    {
        return Encoding.UTF8.GetString(DecodeBytes(sourceBuffer,startOffset,out nextStartOffset));
    }
    
    public static short DecodeShort(byte[] sourceBuffer, int startOffset, out int nextStartOffset)
    {
        var shortBytes = new byte[2];
        Buffer.BlockCopy(sourceBuffer,startOffset,shortBytes,0,2);
        nextStartOffset = startOffset + 2;
        return BitConverter.ToInt16(shortBytes, 0);
    }

    public static int DecodeInt(byte[] sourceBuffer, int startOffset, out int nextStartOffset)
    {
        var intBytes = new byte[4];
        Buffer.BlockCopy(sourceBuffer,startOffset,intBytes,0,4);
        nextStartOffset = startOffset + 4;
        return BitConverter.ToInt32(intBytes, 0);
    }

    public static long DecodeLong(byte[] sourceBuffer, int startOffset, out int nextStratOffset)
    {
        var longBytes = new byte[8];
        Buffer.BlockCopy(sourceBuffer,startOffset,longBytes,0,8);
        nextStratOffset = startOffset + 8;
        return BitConverter.ToInt64(longBytes, 0);
    }

    public static DateTime DecodeDateTime(byte[] sourceBuffer, int startOffset, out int nextStartOffset)
    {
        var longBytes = new byte[8];
        Buffer.BlockCopy(sourceBuffer,startOffset,longBytes,0,8);
        nextStartOffset = startOffset + 8;
        return new DateTime(BitConverter.ToInt64(longBytes, 0));
    }

    public static byte[] DecodeBytes(byte[] sourceBuffer, int startOffset, out int nextStartOffset)
    {
        var lengthBytes = new byte[4];
        Buffer.BlockCopy(sourceBuffer,startOffset,lengthBytes,0,4);
        startOffset += 4;

        var length = BitConverter.ToInt32(lengthBytes, 0);
        var dataBytes = new byte[length];
        Buffer.BlockCopy(sourceBuffer,startOffset,dataBytes,0,length);
        startOffset += length;

        nextStartOffset = startOffset;

        return dataBytes;
    }

    public static byte[] Combine(params byte[][] arrays)
    {
        byte[] destination = new byte[arrays.Sum(x => x.Length)];
        int offset = 0;
        foreach (byte[] data in arrays)
        {
            Buffer.BlockCopy(data,0,destination,offset,data.Length);
            offset += data.Length;
        }

        return destination;
    }

    public static byte[] Combine(IEnumerable<byte[]> arrays)
    {
        byte[] destination = new byte[arrays.Sum(x => x.Length)];
        int offset = 0;
        foreach (byte[] data in arrays)
        {
            Buffer.BlockCopy(data,0,destination,offset,data.Length);
            offset += data.Length;
        }

        return destination;
    }
    
}
