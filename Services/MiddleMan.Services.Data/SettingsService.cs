namespace MiddleMan.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using MiddleMan.Data.Common.Repositories;
    using MiddleMan.Data.Models;

    public class SettingsService : ISettingsService
    {
        private readonly IDeletableEntityRepository<Setting> settingsRepository;

        public SettingsService(IDeletableEntityRepository<Setting> settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        public int GetCount()
        {
            return this.settingsRepository.All().Count();
        }

    }
}
