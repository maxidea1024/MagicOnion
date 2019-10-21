#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Resolvers
{
    using System;
    using MessagePack;

    public class GeneratedResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new GeneratedResolver();

        GeneratedResolver()
        {

        }

        public global::MessagePack.Formatters.IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.formatter;
        }

        static class FormatterCache<T>
        {
            public static readonly global::MessagePack.Formatters.IMessagePackFormatter<T> formatter;

            static FormatterCache()
            {
                var f = GeneratedResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class GeneratedResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static GeneratedResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(5)
            {
                {typeof(global::ChatApp.Shared.MessagePackObjects.RoomDataResponse), 0 },
                {typeof(global::ChatApp.Shared.MessagePackObjects.MatchDataReponse), 1 },
                {typeof(global::ChatApp.Shared.MessagePackObjects.JoinRequest), 2 },
                {typeof(global::ChatApp.Shared.MessagePackObjects.MessageResponse), 3 },
                {typeof(global::ChatApp.Shared.MessagePackObjects.AgonesGameServerResponse), 4 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new MessagePack.Formatters.ChatApp.Shared.MessagePackObjects.RoomDataResponseFormatter();
                case 1: return new MessagePack.Formatters.ChatApp.Shared.MessagePackObjects.MatchDataReponseFormatter();
                case 2: return new MessagePack.Formatters.ChatApp.Shared.MessagePackObjects.JoinRequestFormatter();
                case 3: return new MessagePack.Formatters.ChatApp.Shared.MessagePackObjects.MessageResponseFormatter();
                case 4: return new MessagePack.Formatters.ChatApp.Shared.MessagePackObjects.AgonesGameServerResponseFormatter();
                default: return null;
            }
        }
    }
}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612



#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Formatters.ChatApp.Shared.MessagePackObjects
{
    using System;
    using MessagePack;


    public sealed class RoomDataResponseFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::ChatApp.Shared.MessagePackObjects.RoomDataResponse>
    {

        public int Serialize(ref byte[] bytes, int offset, global::ChatApp.Shared.MessagePackObjects.RoomDataResponse value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 7);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Id, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Host, formatterResolver);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Port);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.ConnectionNumber);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.ConnectionLimit);
            offset += formatterResolver.GetFormatterWithVerify<global::System.DateTimeOffset>().Serialize(ref bytes, offset, value.CreateAt, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref bytes, offset, value.JoinedConnections, formatterResolver);
            return offset - startOffset;
        }

        public global::ChatApp.Shared.MessagePackObjects.RoomDataResponse Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Id__ = default(string);
            var __Host__ = default(string);
            var __Port__ = default(int);
            var __ConnectionNumber__ = default(int);
            var __ConnectionLimit__ = default(int);
            var __CreateAt__ = default(global::System.DateTimeOffset);
            var __JoinedConnections__ = default(string[]);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Id__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Host__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __Port__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __ConnectionNumber__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 4:
                        __ConnectionLimit__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 5:
                        __CreateAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTimeOffset>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 6:
                        __JoinedConnections__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::ChatApp.Shared.MessagePackObjects.RoomDataResponse();
            ____result.Id = __Id__;
            ____result.Host = __Host__;
            ____result.Port = __Port__;
            ____result.ConnectionNumber = __ConnectionNumber__;
            ____result.ConnectionLimit = __ConnectionLimit__;
            ____result.CreateAt = __CreateAt__;
            ____result.JoinedConnections = __JoinedConnections__;
            return ____result;
        }
    }


    public sealed class MatchDataReponseFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::ChatApp.Shared.MessagePackObjects.MatchDataReponse>
    {

        public int Serialize(ref byte[] bytes, int offset, global::ChatApp.Shared.MessagePackObjects.MatchDataReponse value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 3);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.MatchId, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.ClientId, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::ChatApp.Shared.MessagePackObjects.RoomDataResponse>().Serialize(ref bytes, offset, value.Room, formatterResolver);
            return offset - startOffset;
        }

        public global::ChatApp.Shared.MessagePackObjects.MatchDataReponse Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __MatchId__ = default(string);
            var __ClientId__ = default(string);
            var __Room__ = default(global::ChatApp.Shared.MessagePackObjects.RoomDataResponse);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __MatchId__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __ClientId__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __Room__ = formatterResolver.GetFormatterWithVerify<global::ChatApp.Shared.MessagePackObjects.RoomDataResponse>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::ChatApp.Shared.MessagePackObjects.MatchDataReponse();
            ____result.MatchId = __MatchId__;
            ____result.ClientId = __ClientId__;
            ____result.Room = __Room__;
            return ____result;
        }
    }


    public sealed class JoinRequestFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::ChatApp.Shared.MessagePackObjects.JoinRequest>
    {

        public int Serialize(ref byte[] bytes, int offset, global::ChatApp.Shared.MessagePackObjects.JoinRequest value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 2);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.RoomName, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.UserName, formatterResolver);
            return offset - startOffset;
        }

        public global::ChatApp.Shared.MessagePackObjects.JoinRequest Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __RoomName__ = default(string);
            var __UserName__ = default(string);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __RoomName__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __UserName__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::ChatApp.Shared.MessagePackObjects.JoinRequest();
            ____result.RoomName = __RoomName__;
            ____result.UserName = __UserName__;
            return ____result;
        }
    }


    public sealed class MessageResponseFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::ChatApp.Shared.MessagePackObjects.MessageResponse>
    {

        public int Serialize(ref byte[] bytes, int offset, global::ChatApp.Shared.MessagePackObjects.MessageResponse value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 2);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.UserName, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Message, formatterResolver);
            return offset - startOffset;
        }

        public global::ChatApp.Shared.MessagePackObjects.MessageResponse Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __UserName__ = default(string);
            var __Message__ = default(string);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __UserName__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Message__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::ChatApp.Shared.MessagePackObjects.MessageResponse();
            ____result.UserName = __UserName__;
            ____result.Message = __Message__;
            return ____result;
        }
    }


    public sealed class AgonesGameServerResponseFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::ChatApp.Shared.MessagePackObjects.AgonesGameServerResponse>
    {

        public int Serialize(ref byte[] bytes, int offset, global::ChatApp.Shared.MessagePackObjects.AgonesGameServerResponse value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 2);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Host, formatterResolver);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Port);
            return offset - startOffset;
        }

        public global::ChatApp.Shared.MessagePackObjects.AgonesGameServerResponse Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Host__ = default(string);
            var __Port__ = default(int);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Host__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Port__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::ChatApp.Shared.MessagePackObjects.AgonesGameServerResponse();
            ____result.Host = __Host__;
            ____result.Port = __Port__;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
