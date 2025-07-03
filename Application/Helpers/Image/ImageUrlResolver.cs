using AutoMapper;
using DataTransferObjects.Response.User;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers.Image
{
    public class ImageUrlResolver : IValueResolver<TrnUserRegistration, GetUserResponseDTO, string>
    {
        private readonly IImageHelper _imageHelper;

        public ImageUrlResolver(IImageHelper imageHelper)
        {
            _imageHelper = imageHelper;
        }

        public string Resolve(TrnUserRegistration source, GetUserResponseDTO dest, string destMember, ResolutionContext context)
        {
            return _imageHelper.GenerateImageUrl(Constants.UserProfileImages,source.PhotoPath);
        }
    }

}
