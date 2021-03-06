﻿using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Geo
{

    public static class ConfigureExtention
    {
        public static void ConfigureGeoData(this IServiceCollection services)
        {
            services.AddHttpClient(GeocodingService.Geocoding, client =>
            {
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddTransient<IGeocodingService, GeocodingService>();
        }
    }
}