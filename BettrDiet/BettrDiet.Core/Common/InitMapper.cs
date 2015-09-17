using AutoMapper;
using BettrFit.Core.Common;
using BettrFitSPA.Viewmodels;
using BettrFitSPA.Viewmodels.Training;
using BettrFitSPA.Viewmodels.User;

namespace BettrDiet.Core.Common
{
    public class ViewModelProfile : Profile
    {
        protected override void Configure()
        {
        }
    }
    
    /// <summary>
    /// Initialize the Automapper Entities.
    /// </summary>
    public class InitMapper
    {
        /// <summary>
        /// Create all Maps between Objects
        /// </summary>
        public InitMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ViewModelProfile>();

                cfg.CreateMap<WebAccess.ServiceReference.UserGoalVM, UserGoalVM>();
                cfg.CreateMap<UserGoalVM, WebAccess.ServiceReference.UserGoalVM>();
                cfg.CreateMap<UserGoalVM, UserGoalVM>();
                cfg.CreateMap<WebAccess.ServiceReference.UserDailyVM, UserDailyVM>();
                cfg.CreateMap<UserDailyVM, WebAccess.ServiceReference.UserDailyVM>();
                cfg.CreateMap<UserDailyVM, UserDailyVM>();

                cfg.CreateMap<WebAccess.ServiceReference.UserVM, UserVM>();
                cfg.CreateMap<UserVM, WebAccess.ServiceReference.UserVM>();

                cfg.CreateMap<WebAccess.ServiceReference.AchievmentVM, AchievmentVM>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => localTransform(src.Name)))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => localTransform(src.Description)));

                cfg.CreateMap<AchievmentVM, WebAccess.ServiceReference.AchievmentVM>();
                cfg.CreateMap<WebAccess.ServiceReference.AchievmentHistoryVM, AchievmentHistoryVM>();
                cfg.CreateMap<AchievmentHistoryVM, WebAccess.ServiceReference.AchievmentHistoryVM>();

                //Mapper.CreateMap<ObjectId, string>().ConvertUsing(new ObjectIdStringConverter());
                //Mapper.CreateMap<string, ObjectId>().ConvertUsing(new StringObjectIdConverter());

                cfg.CreateMap<PictureVM, WebAccess.ServiceReference.PictureVM>();
                cfg.CreateMap<WebAccess.ServiceReference.PictureVM, PictureVM>();

                cfg.CreateMap<PictureVM, PictureVM>();

                cfg.CreateMap<WebAccess.ServiceReference.LebensmittelVM, LebensmittelVM>()
                                         .ForMember(dest => dest.Name, opt => opt.MapFrom(src => localTransform(src.Name)))
                                         .ForMember(dest => dest.Klasse, opt => opt.MapFrom(src => localTransform(src.Klasse)));

                cfg.CreateMap<LebensmittelVM, WebAccess.ServiceReference.LebensmittelVM>();

                cfg.CreateMap<WebAccess.ServiceReference.LebensmittelConsumedVM, LebensmittelConsumedVM>()
                                         .ForMember(dest => dest.Name, opt => opt.MapFrom(src => localTransform(src.Name)));
                cfg.CreateMap<LebensmittelConsumedVM, WebAccess.ServiceReference.LebensmittelConsumedVM>();

                cfg.CreateMap<WebAccess.ServiceReference.RezeptVM, RezeptVM>()
                                         .ForMember(dest => dest.Name, opt => opt.MapFrom(src => localTransform(src.Name)))
                 .ForMember(dest => dest.Description, opt => opt.MapFrom(src => localTransform(src.Description)));

                cfg.CreateMap<RezeptVM, WebAccess.ServiceReference.RezeptVM>();
                cfg.CreateMap<WebAccess.ServiceReference.IngredientVM, IngredientVM>()
                                         .ForMember(dest => dest.Name, opt => opt.MapFrom(src => localTransform(src.Name)));

                cfg.CreateMap<IngredientVM, WebAccess.ServiceReference.IngredientVM>();
                cfg.CreateMap<WebAccess.ServiceReference.SummaryData, WebAccess.ServiceReference.SummaryData>();

                cfg.CreateMap<NutritionPlanVM, NutritionPlanVM>();
                cfg.CreateMap<Plan1PointsVM, Plan1PointsVM>();
                cfg.CreateMap<WebAccess.ServiceReference.NutritionPlanVM, NutritionPlanVM>()
                     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => localTransform(src.Name)))
                     .ForMember(dest => dest.Description, opt => opt.MapFrom(src => localTransform(src.Description)));

                cfg.CreateMap<NutritionPlanVM, WebAccess.ServiceReference.NutritionPlanVM>();
                cfg.CreateMap<WebAccess.ServiceReference.NutritionPlanDayVM, NutritionPlanDayVM>()
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => localTransform(src.Name)))
                 .ForMember(dest => dest.Description, opt => opt.MapFrom(src => localTransform(src.Description)));

                cfg.CreateMap<NutritionPlanDayVM, WebAccess.ServiceReference.NutritionPlanDayVM>();
                cfg.CreateMap<WebAccess.ServiceReference.NutritionPlanEntryVM, NutritionPlanEntryVM>()
                                         .ForMember(dest => dest.Name, opt => opt.MapFrom(src => localTransform(src.Name)))
                 .ForMember(dest => dest.Description, opt => opt.MapFrom(src => localTransform(src.Description)));

                cfg.CreateMap<NutritionPlanEntryVM, WebAccess.ServiceReference.NutritionPlanEntryVM>();
                cfg.CreateMap<WebAccess.ServiceReference.NutritionPlanFavoriteVM, NutritionPlanFavoriteVM>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => localTransform(src.Name)))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => localTransform(src.Description)));
                    
                cfg.CreateMap<NutritionPlanFavoriteVM, WebAccess.ServiceReference.NutritionPlanFavoriteVM>();
                cfg.CreateMap<WebAccess.ServiceReference.Plan1PointsVM, Plan1PointsVM>();
                cfg.CreateMap<Plan1PointsVM, WebAccess.ServiceReference.Plan1PointsVM>();
                cfg.CreateMap<WebAccess.ServiceReference.DayTimesVM, DayTimesVM>();
                cfg.CreateMap<DayTimesVM, WebAccess.ServiceReference.DayTimesVM>();
                cfg.CreateMap<WebAccess.ServiceReference.LebensmittelPlanEntryVM, LebensmittelPlanEntryVM>();
                cfg.CreateMap<LebensmittelPlanEntryVM, WebAccess.ServiceReference.LebensmittelPlanEntryVM>();

                cfg.CreateMap<BlogPostVM, WebAccess.ServiceReference.BlogPostVM>();
                cfg.CreateMap<WebAccess.ServiceReference.BlogPostVM, BlogPostVM>();
                cfg.CreateMap<BlogCommentEntryVM, WebAccess.ServiceReference.BlogCommentEntryVM>();
                cfg.CreateMap<WebAccess.ServiceReference.BlogCommentEntryVM, BlogCommentEntryVM>();

                cfg.CreateMap<WebAccess.ServiceReference.AchievmentVM, AchievmentVM>()
                   .ForMember(dest => dest.Name, opt => opt.MapFrom(src => localTransform(src.Name)))
                   .ForMember(dest => dest.Description, opt => opt.MapFrom(src => localTransform(src.Description)));
               
            });
        }

        public string localTransform(string src)
        {
            var a = CultureHelper.GetLocalString(src) ?? "";
            //Debug.WriteLine("MapLocal:"+src+"->"+a);
            return a;
        }

        public class LanguageResolver : ValueResolver<string, string>
        {
            protected override string ResolveCore(string source)
            {
                return "NAME." + source;
            }
        }
    }
}