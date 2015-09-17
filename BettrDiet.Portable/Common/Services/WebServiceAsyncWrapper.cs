using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAccess.ServiceReference;


namespace BettrDiet.Core.Common
{

    public static class WebClientAsyncExtensions
    {
        public static Task<ObservableCollection<LebensmittelConsumedVM>> GetConsumedTask(AuthData token, int daytime, DateTime date)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<ObservableCollection<LebensmittelConsumedVM>>();

            //GetConsumedResponse a;


            EventHandler<GetConsumedCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.GetConsumedCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.GetConsumedCompleted += handler;
            client.GetConsumedAsync(token, daytime, date);

            return tcs.Task;
        }

        public static Task<LebensmittelConsumedVM> AddConsumedTask(AuthData authData, string p1, float p2, int daytime, DateTime currentDate)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<LebensmittelConsumedVM>();

            AddConsumedResponse a;


            EventHandler<AddConsumedCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.AddConsumedCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.AddConsumedCompleted += handler;
            client.AddConsumedAsync(authData, p1, p2, daytime, currentDate);

            return tcs.Task;
        }

        public static Task<bool> SetConsumedTask(AuthData authData, LebensmittelConsumedVM cons)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<bool>();

            AddConsumedResponse a;

            EventHandler<SetConsumedCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.SetConsumedCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.SetConsumedCompleted += handler;
            client.SetConsumedAsync(authData, cons);

            return tcs.Task;
        }


        public static Task DeleteConsumedTask(AuthData authData, string p)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<bool>();

            EventHandler<DeleteConsumedCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.DeleteConsumedCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.DeleteConsumedCompleted += handler;
            client.DeleteConsumedAsync(authData, p);

            return tcs.Task;
        }

        public static Task<TrainingDayVM> SetTrainingDayTask(AuthData authData, TrainingDayVM item)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<TrainingDayVM>();

            EventHandler<SetTrainingDayCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.SetTrainingDayCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.SetTrainingDayCompleted += handler;
            client.SetTrainingDayAsync(authData, item);

            return tcs.Task;
        }

        public static Task SetTrainingExerciseTask(AuthData authData, TrainingExerciseVM item)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<TrainingDayVM>();

            EventHandler<SetTrainingExerciseCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.SetTrainingExerciseCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.SetTrainingExerciseCompleted += handler;
            client.SetTrainingExerciseAsync(authData, item);

            return tcs.Task;
        }

        public static Task<UserVM> GetUserProfileTask(AuthData a)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<UserVM>();

            EventHandler<GetUserProfileCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.GetUserProfileCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.GetUserProfileCompleted += handler;
            client.GetUserProfileAsync(a);

            return tcs.Task;
        }

        #region Goals
        public static Task<int> GetUserGoalCountTask(AuthData a)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<int>();

            EventHandler<GetUserGoalCountCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.GetUserGoalCountCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.GetUserGoalCountCompleted += handler;
            client.GetUserGoalCountAsync(a);

            return tcs.Task;
        }

        public static Task<string> AddUserGoalTask(AuthData a, UserGoalVM data)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<string>();

            EventHandler<AddUserGoalCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.AddUserGoalCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.AddUserGoalCompleted += handler;
            client.AddUserGoalAsync(a, data);

            return tcs.Task;
        }


        public static Task<string> UpdateUserGoalTask(AuthData a, UserGoalVM data)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<string>();

            EventHandler<UpdateUserGoalCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.UpdateUserGoalCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.UpdateUserGoalCompleted += handler;
            client.UpdateUserGoalAsync(a, data);

            return tcs.Task;
        }


        public static Task<int> DeleteUserGoalTask(AuthData a, string id)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<int>();

            EventHandler<DeleteUserGoalCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.DeleteUserGoalCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.DeleteUserGoalCompleted += handler;
            client.DeleteUserGoalAsync(a, id);

            return tcs.Task;
        }
#endregion Goals

        public static Task<ArrayOfString> GetClassesTask(AuthData authData)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<ArrayOfString>();

            EventHandler<GetClassesCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.GetClassesCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.GetClassesCompleted += handler;
            client.GetClassesAsync(authData);

            return tcs.Task;
        }

        public static Task<TrainingDayVM> GetCurrentTrainingDayTask(AuthData authData)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<TrainingDayVM>();

            EventHandler<GetCurrentTrainingDayCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.GetCurrentTrainingDayCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.GetCurrentTrainingDayCompleted += handler;
            client.GetCurrentTrainingDayAsync(authData);

            return tcs.Task;
        }

        public static Task<int> GetUserDailyCountTask(AuthData authData)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<int>();

            EventHandler<GetUserDailyCountCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.GetUserDailyCountCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.GetUserDailyCountCompleted += handler;
            client.GetUserDailyCountAsync(authData);

            return tcs.Task;
        }

        public static Task<ObservableCollection<UserDailyVM>> GetUserDailyRangeTask(AuthData authData, int i, int p)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<ObservableCollection<UserDailyVM>>();

            EventHandler<GetUserDailyRangeCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.GetUserDailyRangeCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.GetUserDailyRangeCompleted += handler;
            client.GetUserDailyRangeAsync(authData, i, p);

            return tcs.Task;
        }

        public static Task<string> AddUserDailyTask(AuthData authData, UserDailyVM d)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<string>();

            EventHandler<AddUserDailyCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.AddUserDailyCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.AddUserDailyCompleted += handler;
            client.AddUserDailyAsync(authData, d);

            return tcs.Task;
        }

        public static Task<int> DeleteUserDailyTask(AuthData authData, string id)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<int>();

            EventHandler<DeleteUserDailyCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.DeleteUserDailyCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.DeleteUserDailyCompleted += handler;
            client.DeleteUserDailyAsync(authData, id);

            return tcs.Task;
        }

        public static Task<UserDailyVM> GetUserDailyTask(AuthData authData, string id)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<UserDailyVM>();

            EventHandler<GetUserDailyCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.GetUserDailyCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.GetUserDailyCompleted += handler;
            client.GetUserDailyAsync(authData, id);

            return tcs.Task;
        }



        public static Task<string> UpdateUserDailyTask(AuthData authData, UserDailyVM d)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<string>();

            EventHandler<UpdateUserDailyCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.UpdateUserDailyCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.UpdateUserDailyCompleted += handler;
            client.UpdateUserDailyAsync(authData, d);

            return tcs.Task;
        }


        public static Task<ObservableCollection<UserGoalVM>> GetUseGoalRangeTask(AuthData authData, int i, int p)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<ObservableCollection<UserGoalVM>>();

            EventHandler<GetUserGoalRangeCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.GetUserGoalRangeCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.GetUserGoalRangeCompleted += handler;
            client.GetUserGoalRangeAsync(authData, i, p);

            return tcs.Task;
        }


        public static Task<LebensmittelVM> GetFoodTask(AuthData authData, string id)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<LebensmittelVM>();

            EventHandler<GetFoodCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.GetFoodCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.GetFoodCompleted += handler;            
            client.GetFoodAsync(authData, id);

            return tcs.Task;
        }

        public static Task<SummaryData> GetSummaryTask(AuthData authData, int p, DateTime date)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<SummaryData>();

            EventHandler<GetSummaryCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.GetSummaryCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.GetSummaryCompleted += handler;
            client.GetSummaryAsync(authData, p,date);

            return tcs.Task;
        }

        public static Task<int> SetFoodTask(AuthData authData, LebensmittelVM lebensmittelItem)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<int>();

            
            tcs.SetResult(0);
            
            
            client.SetFoodAsync(authData, lebensmittelItem);

            return tcs.Task;
        }

        public static Task<LebensmittelVM> AddFoodTask(AuthData authData, LebensmittelVM lebensmittelItem)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<LebensmittelVM>();

            EventHandler<AddFoodCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.AddFoodCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.AddFoodCompleted += handler;
            client.AddFoodAsync(authData, lebensmittelItem);

            return tcs.Task;
        }

        public static Task<ObservableCollection<TrainingExerciseVM>> GetExerciseHistoryTask(AuthData authData, TrainingExerciseVM trainingExerciseItem)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<ObservableCollection<TrainingExerciseVM>>();

            EventHandler<GetExerciseHistoryCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.GetExerciseHistoryCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.GetExerciseHistoryCompleted += handler;
            client.GetExerciseHistoryAsync(authData, trainingExerciseItem);

            return tcs.Task;
        }

        public static Task<int> RegisterNewUserTask(RegistrationItem vm)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<int>();

            EventHandler<RegisterNewUserCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.RegisterNewUserCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.RegisterNewUserCompleted += handler;
            client.RegisterNewUserAsync(vm);

            return tcs.Task;
        }

        public static Task<AuthData> AuthenticateUserTask(AuthData a)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<AuthData>();

            EventHandler<AuthenticateUserCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.AuthenticateUserCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.AuthenticateUserCompleted += handler;
            client.AuthenticateUserAsync(a);

            return tcs.Task;
        }


        public static Task<ObservableCollection<NutritionPlanVM>> GetMyNutritionPlansTask(AuthData a)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<ObservableCollection<NutritionPlanVM>>();

            EventHandler<GetMyNutritionPlansCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.GetMyNutritionPlansCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.GetMyNutritionPlansCompleted += handler;
            client.GetMyNutritionPlansAsync(a);

            return tcs.Task;
        }

        public static Task<NutritionPlanVM> GetMyCurrentNutritionPlanTask(AuthData a)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<NutritionPlanVM>();

            EventHandler<GetMyCurrentNutritionPlanCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.GetMyCurrentNutritionPlanCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.GetMyCurrentNutritionPlanCompleted += handler;
            client.GetMyCurrentNutritionPlanAsync(a);

            return tcs.Task;
        }

        public static Task<NutritionPlanVM> GetMyNutritionPlan(AuthData a, string id)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<NutritionPlanVM>();

            EventHandler<GetMyNutritionPlanCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.GetMyNutritionPlanCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.GetMyNutritionPlanCompleted += handler;
            client.GetMyNutritionPlanAsync(a,id);

            return tcs.Task;
        }

        public static Task<ObservableCollection<NutritionPlanVM>> GetNutritionPlans(AuthData a)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<ObservableCollection<NutritionPlanVM>>();

            EventHandler<GetNutritionPlansCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.GetNutritionPlansCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.GetNutritionPlansCompleted += handler;
            client.GetNutritionPlansAsync(a);

            return tcs.Task;
        }

        public static Task<NutritionPlanVM> GetNewNutritionPlanTask(AuthData a, string plan, float cals, DateTime date)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<NutritionPlanVM>();

            EventHandler<GetNewNutritionPlanCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.GetNewNutritionPlanCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.GetNewNutritionPlanCompleted += handler;
            client.GetNewNutritionPlanAsync(a, plan,cals,date);

            return tcs.Task;
        }

        public static Task<int> DeleteNutritionPlanFavoriteTask(AuthData a, string id)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<int>();

            EventHandler<DeleteNutritionPlanFavoriteCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.DeleteNutritionPlanFavoriteCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.DeleteNutritionPlanFavoriteCompleted += handler;
            client.DeleteNutritionPlanFavoriteAsync(a, id);

            return tcs.Task;
        }

        public static Task<NutritionPlanFavoriteVM> SetNutritionPlanFavoriteTask(AuthData a, NutritionPlanFavoriteVM fav)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<NutritionPlanFavoriteVM>();

            EventHandler<SetNutritionPlanFavoriteCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.SetNutritionPlanFavoriteCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.SetNutritionPlanFavoriteCompleted += handler;
            client.SetNutritionPlanFavoriteAsync(a, fav);

            return tcs.Task;
        }

        public static Task<ObservableCollection<NutritionPlanFavoriteVM>> GetNutritionPlanFavoritesTask(AuthData a, string planId)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<ObservableCollection<NutritionPlanFavoriteVM>>();

            EventHandler<GetNutritionPlanFavoritesCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.GetNutritionPlanFavoritesCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.GetNutritionPlanFavoritesCompleted += handler;
            client.GetNutritionPlanFavoritesAsync(a, planId);

            return tcs.Task;
        }

        public static Task<ObservableCollection<LebensmittelVM>> GetNutritionPlanLebensmittelTask(AuthData a, string plan)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<ObservableCollection<LebensmittelVM>>();

            EventHandler<GetNutritionPlanLebensmittelCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.GetNutritionPlanLebensmittelCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.GetNutritionPlanLebensmittelCompleted += handler;
            client.GetNutritionPlanLebensmittelAsync(a, plan);

            return tcs.Task;
        }

        public static Task<Plan1PointsVM> GetRemainingPlan1PointsTask(AuthData a, DateTime date)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<Plan1PointsVM>();

            EventHandler<GetRemainingPlan1PointsCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.GetRemainingPlan1PointsCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            client.GetRemainingPlan1PointsCompleted += handler;
            client.GetRemainingPlan1PointsAsync(a, date);

            return tcs.Task;
        }

        public static Task<AsyncCompletedEventArgs> SendFeedbackTask(AuthData authData, string feedback)
        {
            var client = WebService.Instance.WS;
            var tcs = new TaskCompletionSource<AsyncCompletedEventArgs>();

            EventHandler<AsyncCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                client.SendFeedbackCompleted -= handler;

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e);
                }
            };

            client.SendFeedbackCompleted += handler;
            client.SendFeedbackAsync(authData, feedback);

            return tcs.Task;
        }


    }

}
