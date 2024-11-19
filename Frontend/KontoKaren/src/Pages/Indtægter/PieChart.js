import {Pie} from 'react-chartjs-2';
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from 'chart.js'; 


//pie chart

ChartJS.register(ArcElement, Tooltip, Legend);

const data = {
    labels: [ 'Red', 'Blue', 'Yellow' ],
    datasets: [{
      label: 'My First Dataset',
      data: [300, 50, 100],
      backgroundColor: [
        'rgb(255, 99, 132)',
        'rgb(54, 162, 235)',
        'rgb(255, 205, 86)'
      ],
      hoverOffset: 4
    }]
  };


  function PieChart() {
    return (
      <div className="container">
        <div className="chart-container">
          <Pie data={data} />
        </div>
      </div>
    );
  }

export default PieChart;