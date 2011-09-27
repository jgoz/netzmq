namespace ZeroMQ
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a ZeroMQ context object.
    /// </summary>
    public interface IZmqContext : IDisposable
    {
        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePairSocket"]/*'/>
        IDuplexSocket CreatePairSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePublishExtSocket"]/*'/>
        IDuplexSocket CreatePublishExtSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePublishSocket"]/*'/>
        ISendSocket CreatePublishSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePullSocket"]/*'/>
        IReceiveSocket CreatePullSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePushSocket"]/*'/>
        ISendSocket CreatePushSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreateReplyExtSocket"]/*'/>
        IDuplexSocket CreateReplyExtSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreateReplySocket"]/*'/>
        IDuplexSocket CreateReplySocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreateRequestExtSocket"]/*'/>
        IDuplexSocket CreateRequestExtSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreateRequestSocket"]/*'/>
        IDuplexSocket CreateRequestSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreateSubscribeExtSocket"]/*'/>
        ISubscribeExtSocket CreateSubscribeExtSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreateSubscribeSocket"]/*'/>
        ISubscribeSocket CreateSubscribeSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePollSet"]/*'/>
        IPollSet CreatePollSet(IEnumerable<ISocket> sockets);
    }
}