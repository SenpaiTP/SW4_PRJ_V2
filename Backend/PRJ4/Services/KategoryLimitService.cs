using PRJ4.Models;  
using PRJ4.Repositories;
using PRJ4.DTOs;

namespace PRJ4.Services
{
    public class KategoryLimitService: IKategoryLimitService
    {
        private readonly IKategoryLimitRepo _kategoriLimitRepository;

        private readonly IKategori _kategoriRepository;

        public KategoryLimitService(IKategoryLimitRepo kategoriLimitRepository, IKategori kategoriRepository)
        {
            _kategoriLimitRepository = kategoriLimitRepository;
            _kategoriRepository = kategoriRepository;
        }

         public async Task<KategoryLimitGetDTO> GetByIdKategoryLimitAsync(int id)
        {
            //Check if budget with "id" exists and write error message if not.
            var budget = await _kategoriLimitRepository.GetByIdAsync(id); 
            if (budget == null)
            {
                throw new KeyNotFoundException($"Kategorilimit with id {id} not found.");
            }

            //Check if kategori with foreign key "KategoryId" exists and write error message if not.
            var kategori = await _kategoriRepository.GetByIdAsync(budget.KategoryId);
            if (budget == null)
            {
                throw new KeyNotFoundException($"Kategory with id {budget.KategoryId} not found.");
            }

            //Create kategoriLimit DTO to return
            var budgetReturn = new KategoryLimitGetDTO
            {
                KategoryName = kategori.Navn,
                Limit = budget.Limit
            };
            return budgetReturn;
        }

        public async Task<KategoryLimitGetDTO> AddKategoryLimitAsync(string brugerId, KategoryLimitReturnDTO limitDTO)
        {
            //Check if kategory exists and write error message if not.
            var kategory = await _kategoriRepository.GetByIdAsync(limitDTO.KategoryId);
            
            if(kategory == null) {throw new ArgumentException($"Kategorien: {kategory.Navn} findes ikke.");}
            
            //Create new kategoriLimit
            var budget = new KategoryLimit
            {
                BrugerId = brugerId,
                KategoryId = limitDTO.KategoryId,
                Limit = limitDTO.Limit
            };

            //Add and save the budget
            var createdBudget = await _kategoriLimitRepository.AddAsync(budget);
            await _kategoriLimitRepository.SaveChangesAsync();


            //Create kategoriLimit DTO to return
            var createdBudgetDTO = new KategoryLimitGetDTO
            {
                KategoryName = kategory.Navn,
                Limit = createdBudget.Limit,
     
            };
            return createdBudgetDTO;
        }

    }
}