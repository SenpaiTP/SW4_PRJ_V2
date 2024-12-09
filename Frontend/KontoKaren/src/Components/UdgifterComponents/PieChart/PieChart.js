import './PieChart.css'; 
import React from "react";
import { Pie } from "react-chartjs-2"; 
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from "chart.js"; 

ChartJS.register(ArcElement, Tooltip, Legend); // registrerer moduler til brug i Chart.js

export default function PieChartColors({ chartData }) {
  // genererer en unik farve baseret på en string
  const generateColor = (name) => {
    let hash = 0;
    for (let i = 0; i < name.length; i++) {
      hash = name.charCodeAt(i) + ((hash << 5) - hash);
    }
    const hue = hash % 360; // begrænset til 360 grader (i farvehjulet)
    return `hsl(${hue}, 70%, 50%)`; // 70% saturation, 50% lightness
  };

  // genererer farver for hver indtægt baseret på navn
  const colors = chartData.map((row) => generateColor(row.name));

  // konfigurerer data til Pie Chart
  const data = {
    labels: chartData.map((row) => row.name), // navne på indtægter
    datasets: [
      {
        label: "Udgifter", // dataset-etiket
        data: chartData.map((row) => row.price), // neløb for hver indtægt
        backgroundColor: colors, // genererede farver
        hoverOffset: 4, // effekt ved hover
      },
    ],
  };

  return (
        <Pie data={data} />
  );
}
