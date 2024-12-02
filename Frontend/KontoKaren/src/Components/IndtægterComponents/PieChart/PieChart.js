import './PieChart.css'
import React from "react";
import { Pie } from "react-chartjs-2";
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from "chart.js";
ChartJS.register(ArcElement, Tooltip, Legend);

export default function PieChartColors({ chartData }) {
  const generateColor = (name) => {
    let hash = 0;
    for (let i = 0; i < name.length; i++) {
      hash = name.charCodeAt(i) + ((hash << 5) - hash);
    }
    const hue = hash % 360; // Begræns til 360 grader for HSL farve
    return `hsl(${hue}, 70%, 50%)`; // Returfarve baseret på navn
  };

  const colors = chartData.map((row) => generateColor(row.name));


  const data = {
    labels: chartData.map((row) => row.name), // Labels fra tabellen (indtægtsnavne)
    datasets: [
      {
        label: "Indtægter",
        data: chartData.map((row) => row.price), // Værdier fra tabellen (beløb)
        backgroundColor: colors,
        hoverOffset: 4,
      },
    ],
  };

  return (
    <div className="canvas-container">
      <div className="chart-container">
      <Pie data={data}  />
      </div>
    </div>
  );
}