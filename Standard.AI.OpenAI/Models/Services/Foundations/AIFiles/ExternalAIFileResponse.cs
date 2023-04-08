﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Standard.AI.OpenAI.Models.Services.Foundations.AIFiles
{
    internal class ExternalFileResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("object")]
        public string Object { get; set; }
        [JsonProperty("bytes")]
        public int Bytes { get; set; }
        [JsonProperty("created_at")]
        public int CreatedDate { get; set; }
        [JsonProperty("fileName")]
        public string FileName { get; set; }
        [JsonProperty("purpose")]
        public string Purpose { get; set; }
    }
}
