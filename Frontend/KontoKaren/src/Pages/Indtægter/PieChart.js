import React from "react";
import { Pie } from "react-chartjs-2";
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from "chart.js";

ChartJS.register(ArcElement, Tooltip, Legend);

export default function PieChart({ chartData }) {
  const data = {
    labels: chartData.map((row) => row.name), // Labels fra tabellen (indtægtsnavne)
    datasets: [
      {
        label: "Indtægter",
        data: chartData.map((row) => row.price), // Værdier fra tabellen (beløb)
        backgroundColor: [
          "rgb(255, 99, 132)",
          "rgb(54, 162, 235)",
          "rgb(255, 205, 86)",
        ],
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
