﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace pylorak.TinyWall
{
    [DataContract(Namespace = "TinyWall")]
    public class UpdateModule
    {
        [DataMember]
        public string? Component;

        [DataMember]
        public string? ComponentVersion;

        [DataMember]
        public string? DownloadHash;

        [DataMember]
        public string? UpdateUrl;
    }

    [DataContract(Namespace = "TinyWall")]
    public class UpdateDescriptor : ISerializable<UpdateDescriptor>
    {
        [DataMember]
        public string MagicWord = "TinyWall Update Descriptor";

        [DataMember]
        public UpdateModule[] Modules = Array.Empty<UpdateModule>();

        public JsonTypeInfo<UpdateDescriptor> GetJsonTypeInfo()
        {
            return SourceGenerationContext.Default.UpdateDescriptor;
        }
    }

    public class ServerState
    {
        public bool HasPassword = false;
        public bool Locked = false;
        public UpdateDescriptor? Update = null;
        public FirewallMode Mode = FirewallMode.Unknown;
        public List<MessageType> ClientNotifs = new();
    }
}
