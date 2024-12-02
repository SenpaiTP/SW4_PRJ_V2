import './PieChart.css';
import React from "react";
import { Pie } from "react-chartjs-2";
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from "chart.js";
import { Box, Paper } from "@mui/material";

ChartJS.register(ArcElement, Tooltip, Legend);

export default function PieChartColors({ chartData }) {
  const generateColor = (name) => {
    let hash = 0;
    for (let i = 0; i < name.length; i++) {
      hash = name.charCodeAt(i) + ((hash << 5) - hash);
    }
    const hue = hash % 360; // Limit to 360 degrees for HSL color
    return `hsl(${hue}, 70%, 50%)`; // Return color based on the name
  };

  const colors = chartData.map((row) => generateColor(row.name));

  const data = {
    labels: chartData.map((row) => row.name), // Labels from the table (income names)
    datasets: [
      {
        label: "Indtægter", // Label for the dataset
        data: chartData.map((row) => row.price), // Values from the table (amounts)
        backgroundColor: colors,
        hoverOffset: 4,
      },
    ],
  };

  return (
        <Pie data={data} />
  );
}
