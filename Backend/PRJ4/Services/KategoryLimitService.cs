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

         public async Task<KategoryLimitDTO> GetByIdKategoryLimitAsync(int id)
        {
            var budget = await _kategoriLimitRepository.GetByIdAsync(id); 
            var kategori = await _kategoriRepository.GetByIdAsync(budget.KategoriId);
            if (budget == null)
            {
                throw new KeyNotFoundException($"Budget with id {id} not found.");
            }
            var budgetReturn = new KategoryLimitDTO
            {
                KategoryName = kategori.Navn,
                Limit = budget.Limit
            };

            return budgetReturn;
        }

        public async Task<KategoryLimitDTO> AddKategoryLimitAsync(string brugerId, KategoryLimitDTO limitDTO)
        {
            //Check if kategory exists
            var kategory = await _kategoriRepository.SearchByName(limitDTO.KategoryName);
            
            if(kategory == null) {throw new ArgumentException($"Kategorien: {limitDTO.KategoryName} findes ikke. ");}
            
            //New kategoriLimit
            var budget = new KategoryLimit
            {
                BrugerId = brugerId,
                KategoriId = kategory.KategoriId,
                Limit = limitDTO.Limit
            };

            //Add and save the budget
            var createdBudget = await _kategoriLimitRepository.AddAsync(budget);
            await _kategoriLimitRepository.SaveChangesAsync();


            // Map det oprettede budget til BudgetKategoriDTO
            var createdBudgetDTO = new KategoryLimitDTO
            {
                KategoryName = kategory.Navn,
                Limit = createdBudget.Limit,
     
            };

            //Return created budget
            return createdBudgetDTO;

        }





         // public async Task<List<BudgetKategoriDTO>> GetAllBudgetKategorisAsync()
        // {
        //     //Get all kategory limits
        //     var budgetKategoriListe = await _budgetKategoriRepository.GetAllAsync();
        //     if (budgetKategoriListe == null || !budgetKategoriListe.Any())
        //     {
        //         throw new KeyNotFoundException($"No kategori limits found.");
        //     }


        //     //Get all kategories that have a kategory limit
        //     foreach (var budgetKategori in budgetKategoriListe)
        //     {    
        //         var kategoriListe = await _kategoriRepository.GetByIdAsync(budgetKategori.KategoriId);
        //         if (kategoriListe == null)
        //         {
        //             throw new KeyNotFoundException($"No kategories with id {budgetKategori.KategoriId} found.");
        //         }
        //     }

        //     var budgetReturnListe = new List<BudgetKategoriDTO>();

        //     foreach (var budget in budgetKategoriListe)
        //     {
                
        //         var budgetReturn = new BudgetKategoriDTO
        //         {
        //             //KategoryName = kategoriListe.Navn,
        //             KategoryLimit = budget.KategoryLimit
        //         };

        //         budgetReturnListe.Add(budgetReturn);
        //     }

        //     return budgetReturnListe;
        // }

     



    }
}