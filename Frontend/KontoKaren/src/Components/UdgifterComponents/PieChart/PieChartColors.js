import React from 'react';
import { Pie } from 'react-chartjs-2';
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from 'chart.js';

ChartJS.register(ArcElement, Tooltip, Legend);

// Function to generate unique colors
const generateUniqueColors = (numColors) => {
  const colors = [];
  for (let i = 0; i < numColors; i++) {
    const color = `hsl(${Math.floor((360 / numColors) * i)}, 100%, 50%)`;
    colors.push(color);
  }
  return colors;
};

const PieChartColors = ({ chartData = [] }) => {
  if (!chartData.length) {
    return <div>No data available</div>;
  }

  const uniqueColors = generateUniqueColors(chartData.length);

  const data = {
    labels: chartData.map(item => item.name),
    datasets: [
      {
        data: chartData.map(item => item.price),
        backgroundColor: uniqueColors,
      },
    ],
  };

  return (
    <div className="canvas-container">
      <div className="chart-container">
        <Pie data={data} width={400} height={200} />
      </div>
    </div>
  );
};

export default PieChartColors;