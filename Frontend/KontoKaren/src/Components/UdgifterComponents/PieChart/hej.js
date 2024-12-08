import * as React from 'react';
import { PieChart } from '@mui/x-charts/PieChart';

// Funktion til at generere tilfældige farver
const generateRandomColor = () => {
  const randomHue = Math.floor(Math.random() * 360); // generer tilfældig hue-værdi (0 - 360)
  const randomSaturation = Math.floor(Math.random() * 60) + 40; // random saturation (30 - 80%)
  const randomLightness = Math.floor(Math.random() * 60) + 40; // random lightness (25 - 75%)
  const randomAlpha = (Math.random() * 0.5 + 0.5).toFixed(2); // random alpha-værdi (gennemsigtighed) mellem 0.5 og 1
  
  // Brug backticks (`) for template literal og interpoler værdier korrekt
  return `hsla(${randomHue}, ${randomSaturation}%, ${randomLightness}%, ${randomAlpha})`;
};

export default function PieChartColors({ chartData }) {
  // Hvis chartData er undefined eller ikke et array, skal vi returnere en tom array
  if (!Array.isArray(chartData)) {
    return <div>Ingen data tilgængelige</div>;
  }

  // Generér en farve for hvert datapunkt i chartData
  const colors = chartData.map(() => generateRandomColor());

  // Map chartData til at inkludere farverne
  const data = chartData.map((row, index) => ({
    label: row.name,
    value: row.price,
    color: colors[index],  
  }));

  return (
    <div className="canvas-container">
      <div className="chart-container">
        <PieChart
          series={[{ data }]} 
          width={400}          
          height={200}        
        />
      </div>
    </div>
  );
}
