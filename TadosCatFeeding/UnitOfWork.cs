using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadosCatFeeding.CatFeedingManagement;
using TadosCatFeeding.CatManagement;
using TadosCatFeeding.CatSharingManagement;
using TadosCatFeeding.StatisticProvision;
using TadosCatFeeding.UserManagement;

namespace TadosCatFeeding
{
    public class UnitOfWork
    {
        private readonly string connectionString;
        private UserRepository userRepository;
        private StatisticRepository statisticRepository;
        private CatSharingRepository catSharingRepository;
        private CatFeedingRepository catFeedingRepository;
        private CatRepository catRepository; 

        public UnitOfWork(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public UserRepository UserRepository 
        {  
            get
            {
                if(userRepository == null)
                {
                    userRepository = new UserRepository(connectionString);
                }
                return userRepository;
            }
        }

        public StatisticRepository StatisticRepository
        {
            get
            {
                if (statisticRepository == null)
                {
                    statisticRepository = new StatisticRepository(connectionString);
                }
                return statisticRepository;
            }
        }

        public CatSharingRepository CatSharingRepository
        {
            get
            {
                if (catSharingRepository == null)
                {
                    catSharingRepository = new CatSharingRepository(connectionString);
                }
                return catSharingRepository;
            }
        }

        public CatFeedingRepository CatFeedingRepository
        {
            get
            {
                if (catFeedingRepository == null)
                {
                    catFeedingRepository = new CatFeedingRepository(connectionString);
                }
                return catFeedingRepository;
            }
        }

        public CatRepository CatRepository
        {
            get
            {
                if (catRepository == null)
                {
                    catRepository = new CatRepository(connectionString);
                }
                return catRepository;
            }
        }
    }
}
