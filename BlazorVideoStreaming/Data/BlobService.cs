﻿using Azure.Storage;
using Azure.Storage.Blobs;
using System;

namespace BlazorVideoStreaming.Data
{
    public class BlobService
    {
        // Apenas teste local com emulador
        private readonly string _storageAccount = "devstoreaccount1";
        private readonly string _key = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";

        // Video Container
        private readonly BlobContainerClient _videoContainer;
       
        public BlobService()
        {
            var blobUrl = $"http://127.0.0.1:10000/{_storageAccount}/myvideos";
            var credential = new StorageSharedKeyCredential(_storageAccount, _key);
            _videoContainer = new BlobContainerClient(new(blobUrl), credential);                                                
        }

        public async Task UploadVideo(List<string> filepathvideos, Stream video)
        {
            // Create a container
            await _videoContainer.CreateIfNotExistsAsync();

            foreach (string caminho in filepathvideos)
            {
                await _videoContainer.UploadBlobAsync(caminho, video);
            }

        }
        public async Task<List<Video>> GetVideos()
        {
            var videos = new List<Video>();
            var videoBlobs = _videoContainer.GetBlobsAsync();

            await foreach (var item in videoBlobs)
            {
                var blob = _videoContainer.GetBlobClient(item.Name);
                var url = blob.Uri.ToString();
                videos.Add(new Video { Title = item.Name, Url = url });
            }

            return videos;
        }


    }
}
