namespace ZeroMQ.Options
{
    /// <summary>
    /// Specifies possible socket types for <see cref="Socket"/>.
    /// </summary>
    public enum SocketType
    {
        /// <summary>
        /// ZMQ_PAIR socket type. Can only be connected to a single peer at any one time.
        /// </summary>
        Pair = 0,

        /// <summary>
        /// ZMQ_PUB socket type. Used by a publisher to distribute data.
        /// </summary>
        Pub = 1,

        /// <summary>
        /// ZMQ_SUB socket type. Used by a subscriber to subscribe to data distributed by a publisher
        /// </summary>
        Sub = 2,

        /// <summary>
        /// ZMQ_REQ socket type. Used by a client to send requests to and receive replies from a service.
        /// </summary>
        Req = 3,

        /// <summary>
        /// ZMQ_REP socket type. Used by a service to receive requests from and send replies to a client.
        /// </summary>
        Rep = 4,

        /// <summary>
        /// ZMQ_XREQ socket type. Used for extending request/reply sockets.
        /// </summary>
        XReq = 5,

        /// <summary>
        /// ZMQ_XREP socket type. Used for extending request/reply sockets.
        /// </summary>
        XRep = 6,

        /// <summary>
        /// ZMQ_PULL socket type. Used by a pipeline node to receive messages from upstream pipeline nodes.
        /// </summary>
        Pull = 7,

        /// <summary>
        /// ZMQ_PUSH socket type. Used by a pipeline node to send messages to downstream pipeline nodes.
        /// </summary>
        Push = 8,

        /// <summary>
        /// ZMQ_XPUB socket type. Same as <see cref="Pub"/>, but can receive subscriptions from peers as incoming messages.
        /// </summary>
        XPub = 9,

        /// <summary>
        /// ZMQ_XSUB socket type. Same as <see cref="Sub"/>, but can send subscription messages to publishers.
        /// </summary>
        XSub = 10,

        /// <summary>
        /// ZMQ_ROUTER socket type. Will be implemented in ZMQ v3.0.
        /// </summary>
        Router = 13
    }
}
