import * as React from 'react';
import { PieChart } from '@mui/x-charts/PieChart';

const generateRandomColor = () => {
  const randomHue = Math.floor(Math.random() * 360); // generer tilfældig hue-værdi (0 - 360)
  const randomSaturation = Math.floor(Math.random() * 60) + 40; // random saturation (30 - 80%)
  const randomLightness = Math.floor(Math.random() * 60) + 40; // random lightness (25 - 75%)
  const randomAlpha = (Math.random() * 0.5 + 0.5).toFixed(2); // ramdom alfa-værdi (gennemsigtig) mellem 0.5 og 1
  
  return `hsla(${randomHue}, ${randomSaturation}%, ${randomLightness}%, ${randomAlpha})`; // bruger HSLA i stedet for HSL for at inkludere alfa (gennemsigtighed)
};

export default function PieChartColors({ chartData }) {
  const colors = chartData.map(() => generateRandomColor());

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
