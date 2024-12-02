// design frontpage
import { Box, Button,Typography, FormControlLabel, Checkbox} from '@mui/material';
import React, { useState, useEffect } from 'react';


function DesignFrontPage() {

const [showPieChart, setShowPieChart] = useState(true);

    useEffect(() => {
        const savedPieChart = localStorage.getItem('showPieChart') === 'true';
        setShowPieChart(savedPieChart);
  }, []);

    const handleSaveSettings = () => {
        localStorage.setItem('showPieChart', showPieChart);
        alert('Settings saved!');
    };

    return (
    <Box sx={{ padding: 4 }}>
        <Typography variant="h7" gutterBottom>
        Choose which sections to display on the front page:
        </Typography>

        <Box>
        <FormControlLabel
         control={<Checkbox checked={showPieChart} onChange={(e) => setShowPieChart(e.target.checked)} />}
         label="Show PieChart" />  
        </Box>

        <Box>
            <Button variant="contained" color="primary" onClick={handleSaveSettings}>
                Save Settings
            </Button>
        </Box>
        <Typography variant="h8" gutterBottom>
        Til senere skal ses om der kan gemmes uden en pop up der siger det er gemt 
        </Typography>
    </Box>
    );
} 
export default DesignFrontPage;