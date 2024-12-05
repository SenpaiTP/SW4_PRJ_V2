
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

        public async Task<List<IndstillingerDTO>> GetAllAsync()
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
        }
        public async Task<IndstillingerDTO> AddIndstillingerAsync( IndstillingerDTO indstillingerDTO){

                var indstillinger = new Indstillinger
                {
                    //SetTheme = indstillingerDTO.SetTheme,
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
                    //SetTheme = createdIndstillinger.SetTheme,
                    SetPieChart = createdIndstillinger.SetPieChart,
                    SetSøjlediagram = createdIndstillinger.SetSøjlediagram,
                    SetIndtægter = createdIndstillinger.SetIndtægter,
                    SetUdgifter = createdIndstillinger.SetUdgifter,
                    SetBudget = createdIndstillinger.SetBudget
                };

                return createdIndstillingerDTO;
         }

      /*  public async Task<UpdateThemeDTO> UpdateThemeAsync(UpdateThemeDTO updateThemeDTO)
        {
            var existingTheme = await _indstillingerRepository.
            
        }*/
         
        public async Task<IndstillingerDTO> UpdateIndstillingerAsync( IndstillingerDTO indstillingerDTO)
        {
            var existingIndstillingerList = await _indstillingerRepository.GetAllAsync();

            var existingIndstillinger = existingIndstillingerList.FirstOrDefault();


            //existingIndstillinger.SetTheme = indstillingerDTO.SetTheme;
            existingIndstillinger.SetPieChart = indstillingerDTO.SetPieChart;
            existingIndstillinger.SetSøjlediagram = indstillingerDTO.SetSøjlediagram;
            existingIndstillinger.SetIndtægter = indstillingerDTO.SetIndtægter;
            existingIndstillinger.SetUdgifter = indstillingerDTO.SetUdgifter;
            existingIndstillinger.SetBudget = indstillingerDTO.SetBudget;

            try
            {
                await _indstillingerRepository.Update(existingIndstillinger); 
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
                SetPieChart = existingIndstillinger.SetPieChart,
                SetSøjlediagram = existingIndstillinger.SetSøjlediagram,
                SetIndtægter = existingIndstillinger.SetIndtægter,
                SetUdgifter = existingIndstillinger.SetUdgifter,
                SetBudget = existingIndstillinger.SetBudget

            };

            return updatedIndstillingerDTO;
        }
    }

}