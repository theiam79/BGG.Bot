using AutoMapper;
using BGG.Bot.Core.Models.Collection;
using BGG.Bot.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGG.Bot.Core.Profiles
{
  public class BggApiProfile : Profile
  {
    public BggApiProfile()
    {
      CreateMap<BggCollectionItem, UserCollectionItem>()
        .ForMember(dest => dest.Owned, opt => opt.MapFrom(src => src.CollectionItemStatus.Own))
        .ForMember(dest => dest.PreviouslyOwned, opt => opt.MapFrom(src => src.CollectionItemStatus.PreviouslyOwned))
        .ForMember(dest => dest.ForTrade, opt => opt.MapFrom(src => src.CollectionItemStatus.ForTrade))
        .ForMember(dest => dest.Want, opt => opt.MapFrom(src => src.CollectionItemStatus.Want))
        .ForMember(dest => dest.WantToPlay, opt => opt.MapFrom(src => src.CollectionItemStatus.WantToPlay))
        .ForMember(dest => dest.WantToBuy, opt => opt.MapFrom(src => src.CollectionItemStatus.WantToBuy))
        .ForMember(dest => dest.WishList, opt => opt.MapFrom(src => src.CollectionItemStatus.Wishlist))
        .ForMember(dest => dest.WishlistPriority, opt => opt.MapFrom(src => src.CollectionItemStatus.WishlistPriority))
        .ForMember(dest => dest.PreOrdered, opt => opt.MapFrom(src => src.CollectionItemStatus.Preordered))
        //.ForMember(dest => dest.Rating, opt => opt.ConvertUsing(new FloatTypeConverter(), src => src.BggCollectionItemStats.Rating.Value))
        //.ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => Convert.ToDateTime(src.CollectionItemStatus.LastModified)))
        .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.BggCollectionItemStats.Rating.Value))
        .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => src.CollectionItemStatus.LastModified))
        ;

      CreateMap<string, float>().ConvertUsing<FloatTypeConverter>();
      CreateMap<string, DateTime>().ConvertUsing(s => Convert.ToDateTime(s));
    }
  }

  public class FloatTypeConverter : ITypeConverter<string, float>
  {
    public float Convert(string source, float destination, ResolutionContext context)
    {
      return float.TryParse(source, out float parsed) ? parsed : default;
    }
  }
}
