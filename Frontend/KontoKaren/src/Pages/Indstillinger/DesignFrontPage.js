// design frontpage
import { Check } from '@mui/icons-material';
import { Box, Button, Card, CardContent, Typography, FormControlLabel, Checkbox} from '@mui/material';
//import React from "react";
import React, { useState, useEffect } from 'react';


function DesignFrontPage() {

    //variable til box checked
    const [showPieChart, setShowPieChart] = useState(true);

    useEffect(() => {
        const savedPieChart = localStorage.getItem('showPieChart') === 'true';
        setShowPieChart(savedPieChart); //default er at den er true
  }, []);

    const handleSaveSettings = () => {
        localStorage.setItem('showPieChart', showPieChart); //sender til local storage
        alert('Settings saved!');
    };

    return (
    <Box sx={{ padding: 4 }}>
        <Typography variant="h7" gutterBottom>
        Choose which sections to display on the front page:
        </Typography>

        {/* Gentag denne boks for hver feature der skal tilføjes, label er tekst ved siden af check boks*/}
        <Box>
        <FormControlLabel
         control={<Checkbox checked={showPieChart} onChange={(e) => setShowPieChart(e.target.checked)} />}
         label="Vis PieChart" />  
        </Box>

        <Box>
        <FormControlLabel control={<Checkbox />} label="Indtægter placeholder" />
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