using Microsoft.ML;
using Microsoft.ML.Data;

namespace PRJ4.Servies
{

    public class CategorizerSuggester
    {
        private readonly MLContext _mlContext;
        private PredictionEngine<CategorySuggestion, Prediction> _predictionEngine;

        public CategorizerSuggester()
        {
            _mlContext = new MLContext();

            // Training data: Replace with your own dataset
            var data = new List<CategorySuggestion>
            {
                new CategorySuggestion { Description = "Middag på restaurant", Category = "Mad" },
                new CategorySuggestion { Description = "Indkøb af dagligvarer", Category = "Dagligvarer" },
                new CategorySuggestion { Description = "Køb af en flaske vin", Category = "Alkohol" },
                new CategorySuggestion { Description = "Betaling af husleje", Category = "Husleje" },
                new CategorySuggestion { Description = "Betaling af elregning", Category = "Husleje" },
                new CategorySuggestion { Description = "Månedlig husleje", Category = "Husleje" },
                new CategorySuggestion { Description = "Togbillet til København", Category = "Transport" },
                new CategorySuggestion { Description = "Buskort til skole", Category = "Transport" },
                new CategorySuggestion { Description = "Taxi til lufthavn", Category = "Transport" },
                new CategorySuggestion { Description = "Biografbilletter", Category = "Underholdning" },
                new CategorySuggestion { Description = "Netflix abonnement", Category = "Underholdning" },
                new CategorySuggestion { Description = "Lejede en film på Blockbuster", Category = "Underholdning" },
                new CategorySuggestion { Description = "Køb af tøj i H&M", Category = "Tøj" },
                new CategorySuggestion { Description = "Sko fra Zalando", Category = "Tøj" },
                new CategorySuggestion { Description = "Indkøb af elektronik", Category = "Elektronik" },
                new CategorySuggestion { Description = "Reparation af bilen", Category = "Bil" },
                new CategorySuggestion { Description = "Brændstof til bilen", Category = "Bil" },
                new CategorySuggestion { Description = "Bilvask i weekenden", Category = "Bil" },
                new CategorySuggestion { Description = "Gave til fødselsdag", Category = "Gaver" },
                new CategorySuggestion { Description = "Julegave til familien", Category = "Gaver" },
                new CategorySuggestion { Description = "Blomster til en ven", Category = "Gaver" },
                new CategorySuggestion { Description = "Besøg hos frisøren", Category = "Personlig Pleje" },
                new CategorySuggestion { Description = "Køb af hudplejeprodukter", Category = "Personlig Pleje" },
                new CategorySuggestion { Description = "Tandlægebesøg", Category = "Personlig Pleje" },
                new CategorySuggestion { Description = "Betaling for tandlægeforsikring", Category = "Forsikring" },
                new CategorySuggestion { Description = "Årlig bilforsikring", Category = "Forsikring" },
                new CategorySuggestion { Description = "Indboforsikring", Category = "Forsikring" },
                new CategorySuggestion { Description = "Bog til studiet", Category = "Uddannelse" },
                new CategorySuggestion { Description = "Kursusgebyr for sprogkursus", Category = "Uddannelse" },
                new CategorySuggestion { Description = "Køb af kontorartikler", Category = "Kontorartikler" },
                new CategorySuggestion { Description = "Donerede penge til Røde Kors", Category = "Donationer" },
                new CategorySuggestion { Description = "Indsamling til velgørenhed", Category = "Donationer" },
                new CategorySuggestion { Description = "Bidrag til børnehjælp", Category = "Donationer" },
                new CategorySuggestion { Description = "Cykelreparation", Category = "Transport" },
                new CategorySuggestion { Description = "Køb af månedskort til bus", Category = "Transport" },
                new CategorySuggestion { Description = "Køb af en ny cykel", Category = "Shopping" },
                new CategorySuggestion { Description = "Kaffe og kage på café", Category = "Mad" },
                new CategorySuggestion { Description = "Weekendophold på hotel", Category = "Underholdning" },
                new CategorySuggestion { Description = "Køb af fitnessmedlemskab", Category = "Personlig Pleje" },
                new CategorySuggestion { Description = "Køb af headset", Category = "Elektronik" },
                new CategorySuggestion { Description = "computer Mus", Category = "Computer" },
                new CategorySuggestion { Description = "Medlemskab til Fitness", Category = "Abonment" },
                new CategorySuggestion { Description = "Netflix", Category = "Abonment" },
                new CategorySuggestion { Description = "Købte mælk og brød i Netto", Category = "Dagligvarer" },
                new CategorySuggestion { Description = "Betalte for en busbillet til Aarhus", Category = "Transport" },
                new CategorySuggestion { Description = "Gik i biografen og så en film", Category = "Underholdning" },
                new CategorySuggestion { Description = "Betaling af elregning for denne måned", Category = "Forsyninger" },
                new CategorySuggestion { Description = "Husleje for november måned", Category = "Husleje" },
                new CategorySuggestion { Description = "Middag på restaurant med venner", Category = "Mad" },
                new CategorySuggestion { Description = "Købte vitaminer og medicin på apoteket", Category = "Sundhed" },
                new CategorySuggestion { Description = "Køb af en ny jakke i H&M", Category = "Shopping" },
                new CategorySuggestion { Description = "Flybillet til Paris", Category = "Rejser" },
                new CategorySuggestion { Description = "Køb af studiebøger til universitetet", Category = "Uddannelse" },
                new CategorySuggestion { Description = "Medlemskab af fitnesscenter", Category = "Fitness" },
                new CategorySuggestion { Description = "Købte en gave til min ven", Category = "Gaver" },
                new CategorySuggestion { Description = "Abonnement på Netflix", Category = "Abonnementer" },
                new CategorySuggestion { Description = "Skiftede olie på bilen", Category = "Bilvedligeholdelse" },
                new CategorySuggestion { Description = "Betaling af bilforsikring", Category = "Forsikring" },
                new CategorySuggestion { Description = "Betaling af dagpleje for mit barn", Category = "Børnepasning" },
                new CategorySuggestion { Description = "Overført penge til opsparing", Category = "Opsparing" },
                new CategorySuggestion { Description = "Månedlig lønudbetaling", Category = "Løn" },
                new CategorySuggestion { Description = "Købte aktier i Vestas", Category = "Investering" },
                new CategorySuggestion { Description = "Betaling af telefon- og internetregning", Category = "Telefoni & Internet" },
                new CategorySuggestion { Description = "Gave til velgørenhed", Category = "Donationer" },
                new CategorySuggestion { Description = "Købte rengøringsmidler og toiletpapir", Category = "Husholdning" },
                new CategorySuggestion { Description = "Købte foder til min hund", Category = "Kæledyr" },
                new CategorySuggestion { Description = "Reparerede vaskemaskinen", Category = "Reparationer" },
                new CategorySuggestion { Description = "Afbetaling af studielån", Category = "Lån" },
                new CategorySuggestion { Description = "Drikkevarer med kolleger på en bar", Category = "Bar & Alkohol" },
                new CategorySuggestion { Description = "Købte shampoo og ansigtscreme", Category = "Personlig Pleje" },
                new CategorySuggestion { Description = "Betaling af årlig skat", Category = "Skat" },
                new CategorySuggestion { Description = "Købte en ny bærbar computer", Category = "Elektronik" },
                new CategorySuggestion { Description = "Køb af en sofa til stuen", Category = "Møbler" },
                new CategorySuggestion { Description = "Købte maling og pensler til hobbyprojekt", Category = "Hobby" },
                new CategorySuggestion { Description = "Køb af fodboldudstyr", Category = "Sport" },
                new CategorySuggestion { Description = "Købte planter og havejord", Category = "Have" },
                new CategorySuggestion { Description = "Betaling af tandlægeregning", Category = "Sundhed" },
                new CategorySuggestion { Description = "Betalte for arbejdsrelaterede materialer", Category = "Arbejde" },
                new CategorySuggestion { Description = "Diverse småindkøb", Category = "Diverse" }

            };


            var trainingData = _mlContext.Data.LoadFromEnumerable(data);

            // Create a pipeline
            var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(CategorySuggestion.Description))
                .Append(_mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(CategorySuggestion.Category)))
                .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy())
                .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            // Train the model
            var model = pipeline.Fit(trainingData);

            // Create a prediction engine
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<CategorySuggestion, Prediction>(model);
        }

        public string PredictCategory(string description)
        {
            var prediction = _predictionEngine.Predict(new CategorySuggestion { Description = description });
            Console.WriteLine(prediction.PredictedCategory);
            return prediction.PredictedCategory;
        }
    }

    // Input data class
    public class CategorySuggestion
    {
        [LoadColumn(0)]
        public string Description { get; set; }

        [LoadColumn(1)]
        public string Category { get; set; }
    }

    // Prediction class
    public class Prediction
    {
        [ColumnName("PredictedLabel")]
        public string PredictedCategory { get; set; }
    }
 
}