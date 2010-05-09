﻿using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Norm
{
    //todo: cleanup, timeout, age hanlding

    /// <summary>
    /// TCP client MongoDB connection
    /// </summary>
    public class Connection : IConnection, IOptionsContainer
    {
        private readonly ConnectionStringBuilder _builder;
        private readonly TcpClient _client;
        private bool _disposed;
        private int? _queryTimeout;
        private bool? _strictMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        /// <param name="builder">The builder.</param>
        internal Connection(ConnectionStringBuilder builder)
        {
            _builder = builder;
            Created = DateTime.Now;
            _client = new TcpClient
            {
                NoDelay = true,
                ReceiveTimeout = builder.QueryTimeout * 1000,
                SendTimeout = builder.QueryTimeout * 1000
            };
            _client.Connect(builder.Servers[0].Host, builder.Servers[0].Port);
            this.ConnectionString = builder.ToString();
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <value>The client.</value>
        public TcpClient Client
        {
            get { return _client; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is connected.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected
        {
            get { return Client.Connected; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is invalid.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is invalid; otherwise, <c>false</c>.
        /// </value>
        public bool IsInvalid { get; private set; }

        /// <summary>
        /// Gets the created date and time.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; private set; }

        /// <summary>
        /// Gets the query timeout.
        /// </summary>
        /// <value>The query timeout.</value>
        public int QueryTimeout
        {
            get { return _queryTimeout ?? _builder.QueryTimeout; }
        }

        /// <summary>
        /// Gets a value indicating whether to use strict mode.
        /// </summary>
        /// <value><c>true</c> if strict mode; otherwise, <c>false</c>.</value>
        public bool StrictMode
        {
            get { return _strictMode ?? _builder.StrictMode; }
        }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName
        {
            get { return _builder.UserName; }
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>The database.</value>
        public string Database
        {
            get { return _builder.Database; }
        }

        /// <summary>
        /// Digests the specified nonce.
        /// </summary>
        /// <param name="nonce">The nonce.</param>
        /// <returns></returns>
        public string Digest(string nonce)
        {
            using (var md5 = MD5.Create())
            {
                var rawDigest = Encoding.UTF8.GetBytes(string.Concat(nonce, UserName, _builder.Password));
                var hashed = md5.ComputeHash(rawDigest);
                var sb = new StringBuilder(hashed.Length * 2);
                Array.ForEach(hashed, b => sb.Append(b.ToString("X2")));
                return sb.ToString().ToLower();
            }
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <returns></returns>
        public NetworkStream GetStream()
        {
            return Client.GetStream();
        }

        /// <summary>
        /// Loads the options.
        /// </summary>
        /// <param name="options">The options.</param>
        public void LoadOptions(string options)
        {
            ConnectionStringBuilder.BuildOptions(this, options);
        }

        /// <summary>
        /// Resets the options.
        /// </summary>
        public void ResetOptions()
        {
            _queryTimeout = null;
            _strictMode = null;
        }

        /// <summary>
        /// Writes the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="start">The start.</param>
        /// <param name="size">The size.</param>
        public void Write(byte[] bytes, int start, int size)
        {
            try
            {
                GetStream().Write(bytes, 0, size);
            }
            catch (IOException)
            {
                IsInvalid = true;
                throw;
            }
        }

        /// <summary>
        /// Sets the query timeout.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        public void SetQueryTimeout(int timeout)
        {
            _queryTimeout = timeout;
        }


        /// <summary>
        /// Sets the strict mode.
        /// </summary>
        /// <param name="strict">if set to <c>true</c> [strict].</param>
        public void SetStrictMode(bool strict)
        {
            _strictMode = strict;
        }

        /// <summary>
        /// Sets the size of the pool.
        /// </summary>
        /// <param name="size">The size.</param>
        public void SetPoolSize(int size)
        {
            throw new MongoException("PoolSize cannot be provided as an override option");
        }

        /// <summary>
        /// Sets the pooled.
        /// </summary>
        /// <param name="pooled">if set to <c>true</c> [pooled].</param>
        public void SetPooled(bool pooled)
        {
            throw new MongoException("Connection pooling cannot be provided as an override option");
        }

        /// <summary>
        /// Sets the timeout.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        public void SetTimeout(int timeout)
        {
            throw new MongoException("Timeout cannot be provided as an override option");
        }

        /// <summary>
        /// Sets the lifetime.
        /// </summary>
        /// <param name="lifetime">The lifetime.</param>
        public void SetLifetime(int lifetime)
        {
            throw new MongoException("Lifetime cannot be provided as an override option");
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _client.Close();
            _disposed = true;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="Connection"/> is reclaimed by garbage collection.
        /// </summary>
        ~Connection()
        {
            Dispose(false);
        }

        public string ConnectionString
        {
            get;
            private set;
        }
    }
}
