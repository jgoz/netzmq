namespace ZeroMQ
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a ZeroMQ context object.
    /// </summary>
    public interface IZmqContext : IDisposable
    {
        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreateDealerSocket"]/node()[name()!="exception"]'/>
        IDuplexSocket CreateDealerSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePairSocket"]/node()[name()!="exception"]'/>
        IDuplexSocket CreatePairSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePublishExtSocket"]/node()[name()!="exception"]'/>
        IDuplexSocket CreatePublishExtSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePublishSocket"]/node()[name()!="exception"]'/>
        ISendSocket CreatePublishSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePullSocket"]/node()[name()!="exception"]'/>
        IReceiveSocket CreatePullSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePushSocket"]/node()[name()!="exception"]'/>
        ISendSocket CreatePushSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreateReplyExtSocket"]/node()[name()!="exception"]'/>
        IDuplexSocket CreateReplyExtSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreateReplySocket"]/node()[name()!="exception"]'/>
        IDuplexSocket CreateReplySocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreateRequestExtSocket"]/node()[name()!="exception"]'/>
        IDuplexSocket CreateRequestExtSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreateRequestSocket"]/node()[name()!="exception"]'/>
        IDuplexSocket CreateRequestSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreateRouterSocket"]/node()[name()!="exception"]'/>
        IDuplexSocket CreateRouterSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreateSubscribeExtSocket"]/node()[name()!="exception"]'/>
        ISubscribeSocket CreateSubscribeExtSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreateSubscribeSocket"]/node()[name()!="exception"]'/>
        ISubscribeSocket CreateSubscribeSocket();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePollSet"]/node()[name()!="exception"]'/>
        IPollSet CreatePollSet(IEnumerable<ISocket> sockets);

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Terminate"]/node()[name()!="exception" and name!="remarks"]'/>
        void Terminate();
    }
}