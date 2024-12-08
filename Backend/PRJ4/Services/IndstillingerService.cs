
using PRJ4.Models;  
using PRJ4.Repositories;
using PRJ4.DTOs;

namespace PRJ4.Services
{

    public class IndstillingerService : IIndstillingerService
    {
        private readonly IIndstillingerRepo _indstillingerRepository;

        public IndstillingerService(IIndstillingerRepo indstillingerRepo)
        {
            _indstillingerRepository = indstillingerRepo;
        }

       /* public async Task<List<IndstillingerDTO>> GetIndstillingerAsync()
        {
            var indstillingerListe = await _indstillingerRepository.GetAllAsync();

            var indstillingerReturnListe = new List<IndstillingerDTO>();
            
            foreach(var indstillinger in indstillingerListe){
               
                var indstillingerReturn = new IndstillingerDTO
                {
                    //SetTheme = indstillinger.SetTheme,
                    SetPieChart = indstillinger.SetPieChart,
                    SetSøjlediagram = indstillinger.SetSøjlediagram,
                    SetIndtægter = indstillinger.SetIndtægter,
                    SetUdgifter = indstillinger.SetUdgifter,
                    SetBudget = indstillinger.SetBudget
                };

                indstillingerReturnListe.Add(indstillingerReturn);
            }

            return indstillingerReturnListe;
        }*/

        public async Task<IndstillingerDTO> AddIndstillingerAsync( string userId, IndstillingerDTO indstillingerDTO){

                var indstillinger = new Indstillinger
                {
                    BrugerId = userId,
                    SetPieChart = indstillingerDTO.SetPieChart,
                    SetSøjlediagram = indstillingerDTO.SetSøjlediagram,
                    SetIndtægter = indstillingerDTO.SetIndtægter,
                    SetUdgifter = indstillingerDTO.SetUdgifter,
                    SetBudget = indstillingerDTO.SetBudget
                };

                var createdIndstillinger = await _indstillingerRepository.AddAsync(indstillinger);
                await _indstillingerRepository.SaveChangesAsync();

                var createdIndstillingerDTO = new IndstillingerDTO
                {
                    SetPieChart = createdIndstillinger.SetPieChart,
                    SetSøjlediagram = createdIndstillinger.SetSøjlediagram,
                    SetIndtægter = createdIndstillinger.SetIndtægter,
                    SetUdgifter = createdIndstillinger.SetUdgifter,
                    SetBudget = createdIndstillinger.SetBudget
                };

                return createdIndstillingerDTO;
         }
        
        public async Task<Indstillinger> AddThemeAsync(string userId, UpdateThemeDTO updateThemeDTO)
        {
           /* var indstillinger = new Indstillinger
            {
                BrugerId = userId,
                SetTheme = updateThemeDTO.SetTheme
            };
            var createdIndstillinger = await _indstillingerRepository.AddAsync(indstillinger);
            await _indstillingerRepository.SaveChangesAsync();

            var createdIndstillingerDTO = new UpdateThemeDTO
            {
                SetTheme = createdIndstillinger.SetTheme
            };*/

            return await _indstillingerRepository.AddThemeAsync(userId, updateThemeDTO);
        }


        public async Task<UpdateThemeDTO> UpdateThemeAsync(string userId, int id, UpdateThemeDTO updateThemeDTO)
        {
            var indstillinger = await _indstillingerRepository.GetByIdAsync(id);

            indstillinger.SetTheme = updateThemeDTO.SetTheme;

             try
            {
                await _indstillingerRepository.Update(indstillinger); 
                await _indstillingerRepository.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine($" Error updating indstillinger: {ex.Message}");
                return null;
            }
            
           var updatedThemeDTO = new UpdateThemeDTO
           {
                SetTheme = indstillinger.SetTheme
           };

            return updatedThemeDTO;  
        }
         
        public async Task<IndstillingerDTO> UpdateIndstillingerAsync( string userId, int id, IndstillingerDTO indstillingerDTO)
        {
            var existingIndstillingerList = await _indstillingerRepository.GetByIdAsync(id);

            if(indstillingerDTO == null)
            {
                throw new Exception("ingen indstillinger");
            }

            existingIndstillingerList.SetPieChart = indstillingerDTO.SetPieChart;
            existingIndstillingerList.SetSøjlediagram = indstillingerDTO.SetSøjlediagram;
            existingIndstillingerList.SetIndtægter = indstillingerDTO.SetIndtægter;
            existingIndstillingerList.SetUdgifter = indstillingerDTO.SetUdgifter;
            existingIndstillingerList.SetBudget = indstillingerDTO.SetBudget;

            try
            {
                await _indstillingerRepository.Update(existingIndstillingerList); 
                await _indstillingerRepository.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine($" Error updating indstillinger: {ex.Message}");
                return null;
            }

            var updatedIndstillingerDTO = new IndstillingerDTO
            {
                //SetTheme = existingIndstillinger.SetTheme,
                SetPieChart = existingIndstillingerList.SetPieChart,
                SetSøjlediagram = existingIndstillingerList.SetSøjlediagram,
                SetIndtægter = existingIndstillingerList.SetIndtægter,
                SetUdgifter = existingIndstillingerList.SetUdgifter,
                SetBudget = existingIndstillingerList.SetBudget

            };

            return updatedIndstillingerDTO;
        }
    }

}