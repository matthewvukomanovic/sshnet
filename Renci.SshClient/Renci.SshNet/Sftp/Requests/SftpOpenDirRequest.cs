﻿using System;
using System.Text;
using Renci.SshNet.Sftp.Responses;

namespace Renci.SshNet.Sftp.Requests
{
    internal class SftpOpenDirRequest : SftpRequest
    {
#if TUNING
        private byte[] _path;
#endif

        public override SftpMessageTypes SftpMessageType
        {
            get { return SftpMessageTypes.OpenDir; }
        }

#if TUNING
        public string Path
        {
            get { return Encoding.GetString(_path, 0, _path.Length); }
            private set { _path = Encoding.GetBytes(value); }
        }
#else
        public string Path { get; private set; }
#endif

        public Encoding Encoding { get; private set; }

#if TUNING
        /// <summary>
        /// Gets the size of the message in bytes.
        /// </summary>
        /// <value>
        /// The size of the messages in bytes.
        /// </value>
        protected override int BufferCapacity
        {
            get
            {
                var capacity = base.BufferCapacity;
                capacity += 4; // Path length
                capacity += _path.Length; // Path
                return capacity;
            }
        }
#endif

        public SftpOpenDirRequest(uint protocolVersion, uint requestId, string path, Encoding encoding, Action<SftpHandleResponse> handleAction, Action<SftpStatusResponse> statusAction)
            : base(protocolVersion, requestId, statusAction)
        {
            Encoding = encoding;
            Path = path;
            SetAction(handleAction);
        }

        protected override void LoadData()
        {
            base.LoadData();

#if TUNING
            _path = ReadBinary();
#else
            Path = ReadString(Encoding);
#endif
        }

        protected override void SaveData()
        {
            base.SaveData();

#if TUNING
            WriteBinaryString(_path);
#else
            Write(Path, Encoding);
#endif
        }
    }
}
