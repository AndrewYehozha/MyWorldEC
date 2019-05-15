using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplication.Models;
using WebApplication.Models.Request;
using WebApplication.Models.Response;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Authorize]
    public class PreferencesController : ApiController
    {
        private PreferenceService _preferenceService = new PreferenceService();

        // GET: api/Preferences
        [ActionName("GetPreferences")]
        [HttpGet]
        public async Task<object> GetPreferences()
        {
            var preferences = await _preferenceService.GetPreferences();

            if (preferences == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Preferences not found");
            }

            var models = new List<PreferenceViewModel>();

            foreach (var preference in preferences)
            {
                models.Add(GetPreferenceModel(preference));
            }

            return JsonResults.Success(models);
        }

        // GET: api/Preferences/5
        [ActionName("GetPreference")]
        [HttpGet]
        public async Task<object> GetPreference(int id)
        {
            var preference = await _preferenceService.GetPreference(id);

            if (preference == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "It`s preference not found");
            }

            var model = GetPreferenceModel(preference);

            return JsonResults.Success(model);
        }

        // GET: api/Preferences/5
        [ActionName("GetPreferenceByUserId")]
        [HttpGet]
        public async Task<object> GetPreferenceByUserId(int id)
        {
            var preferences = await _preferenceService.GetPreferenceByUserId(id);

            if (preferences == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Preferences not found");
            }

            var models = new List<PreferenceViewModel>();

            foreach (var preference in preferences)
            {
                models.Add(GetPreferenceModel(preference));
            }

            return JsonResults.Success(models);
        }

        // GET: api/Services/5
        [ActionName("GetServiceNoExistToPreferenceByUserId")]
        [HttpGet]
        public async Task<object> GetServiceNoExistToPreferenceByUserId(int id)
        {
            var services = await _preferenceService.GetServiceNoExistToPreferenceByUserId(id);

            if (services == null)
            {
                return JsonResults.Error();
            }

            var models = new List<PreferenceViewModel>();

            foreach (var service in services)
            {
                models.Add(
                    new PreferenceViewModel
                    {
                        ServiceId = service.Id,
                        ServiceName = service.Name
                    }
                );
            }

            return JsonResults.Success(models);
        }

        // PUT: api/Preferences/5
        [ActionName("AddEditPreference")]
        [HttpPost]
        public async Task<object> AddEditPreference(PreferenceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return JsonResults.Error(400, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage.ToString());
            }

            try
            {
                var checkPreference = await _preferenceService.CheckPreference(request);

                if (!checkPreference)
                {
                    var model = new Preference
                    {
                        UserId = request.UserId,
                        ServiceId = request.ServiceId,
                        Checked = true
                    };

                    await _preferenceService.AddPreference(model);

                    return JsonResults.Success(GetPreferenceModel(model));
                }

                var preference = await _preferenceService.GetPreference(request.Id);

                preference.ServiceId = request.ServiceId;
                preference.Checked = true;

                await _preferenceService.UpdatePreference(preference);

                return JsonResults.Success(GetPreferenceModel(preference));
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        // DELETE: api/Preferences/5
        [ResponseType(typeof(Preference))]
        [ActionName("DeletePreference")]
        [HttpDelete]
        public async Task<object> DeletePreference(int id)
        {
            var preference = await _preferenceService.GetPreference(id);

            if (preference == null)
            {
                return JsonResults.Error();
            }

            await _preferenceService.DeletePreference(preference);

            return JsonResults.Success();
        }

        // Util
        private PreferenceViewModel GetPreferenceModel(Preference preference)
        {
            var model = new PreferenceViewModel
            {
                Id = preference.Id,
                UserId = preference.UserId,
                ServiceId = preference.ServiceId,
                ServiceName = preference.Service.Name
            };

            return model;
        }
    }
}