using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESozluk.Domain.DTOs;
using ESozluk.Domain.Entities;

namespace ESozluk.Business.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();
            CreateMap<Category, CategoryResponse>();
            CreateMap<Category, CategoryWithTopicsResponse>()
                .ForMember(dest => dest.Topics, opt =>
                opt.MapFrom(src => src.Topics));


            CreateMap<AddUserRequest, User>();
            CreateMap<UpdateUserRequest, User>();
            CreateMap<User, UserResponse>();
            CreateMap<AddComplaintRequest, EntryComplaint>();


            CreateMap<AddTopicRequest, Topic>();
            CreateMap<UpdateTopicRequest, Topic>();
            //category ve user
            CreateMap<Topic, TopicResponse>()// CategoryName alanını doldururken, eğer Product'a bağlı bir Category varsa adını, yoksa hata vermemesi için "Kategori Yok" yaz.
                .ForMember(dest => dest.UserName, opt =>
                    opt.MapFrom(src => src.User != null ? src.User.FullName : "Kullanıcı Yok"))
                .ForMember(dest => dest.CategoryName, opt =>
                    opt.MapFrom(src => src.Category != null ? src.Category.Name : "Kategori Yok"));
            CreateMap<Topic, TopicWithEntriesResponse>()
                .ForMember(dest => dest.Entries, opt =>
                opt.MapFrom(src => src.Entries));

            CreateMap<AddLikeRequest, Like>();
            //User ve entry
            CreateMap<Like, LikeResponse>()
                .ForMember(dest => dest.UserName, opt =>
                    opt.MapFrom(src => src.User != null ? src.User.FullName : "Kullanıcı Yok"))
                .ForMember(dest => dest.EntryName, opt =>
                    opt.MapFrom(src => src.Entry != null ? src.Entry.Name : "Entry Yok"));

            CreateMap<AddEntryRequest, Entry>();
            CreateMap<UpdateEntryRequest, Entry>();
            CreateMap<Entry, EntryResponse>()
                .ForMember(dest => dest.UserName, opt =>
                    opt.MapFrom(src => src.User != null ? src.User.FullName : "Kullanıcı Yok"))
                .ForMember(dest => dest.TopicName, opt =>
                    opt.MapFrom(src => src.Topic != null ? src.Topic.Name : "Topic Yok"))
                .ForMember(dest => dest.LikeCount, opt =>
                     opt.MapFrom(src => src.Likes != null ? src.Likes.Count : 0));

            CreateMap<EntryComplaint, ComplaintResponse>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.EntryName, opt => opt.MapFrom(src => src.Entry.Name));

            CreateMap<User, LoginResponse>();


        }

    }
}
