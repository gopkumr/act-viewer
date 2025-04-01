﻿using System.Security.Cryptography.Xml;
using System;

namespace ACRViewer.BlazorServer.Configuration
{
    public class ACRSettings
    {
        public static string SectionName = "AzureACR";
        public string Name { get; set; } = "";
        public string BaseUrl { get; set; } = "";

        public string TagManifestEndpoint => "/v2/{image}/manifests/{tag}";
    }
}
