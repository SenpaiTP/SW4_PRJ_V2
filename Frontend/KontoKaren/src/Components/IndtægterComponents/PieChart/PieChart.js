import React from "react";
import { Pie } from "react-chartjs-2";
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from "chart.js";

ChartJS.register(ArcElement, Tooltip, Legend);

export default function PieChart({ chartData }) {
  const colors = chartData.map(() => 
    `hsl(${Math.random() * 360}, 70%, 50%)`
  );
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
    <div>
      <Pie data={data} />
    </div>
  );
}
