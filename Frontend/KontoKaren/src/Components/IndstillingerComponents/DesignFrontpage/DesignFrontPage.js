// design frontpage
import { Check } from '@mui/icons-material';
import { Box, Button, Card, CardContent, Typography, FormControlLabel, Checkbox} from '@mui/material';
//import React from "react";
import React, { useState, useEffect } from 'react';


function DesignFrontPage() {

    //variable til box checked
    const [showPieChart, setShowPieChart] = useState(true);
    const [showBudget, setShowBudget] = useState(true);
    const [showIndtægter, setShowIndtægter] = useState(true);
    const [showUdgifter, setShowUdgifter] = useState(true);
    const [showSøjlediagram, setShowSøjlediagram] = useState(true);

    useEffect(() => {
        const savedPieChart = localStorage.getItem('showPieChart') === 'true';
        const savedBudget = localStorage.getItem('showBudget') === 'true';
        const savedIndtægter = localStorage.getItem('showIndtægter') === 'true';
        const savedUdgifter = localStorage.getItem('showUdgifter') === 'true';
        const savedSøjlediagram = localStorage.getItem('showSøjlediagram') === 'true';

        setShowPieChart(savedPieChart); //default er at den er true
        setShowBudget(savedBudget); 
        setShowIndtægter(savedIndtægter);
        setShowUdgifter(savedUdgifter);
        setShowSøjlediagram(savedSøjlediagram);
  }, []);

    const handleSaveSettings = () => {
        localStorage.setItem('showPieChart', showPieChart); //sender til local storage
        localStorage.setItem('showBudget', showBudget);
        localStorage.setItem('showIndtægter', showIndtægter);
        localStorage.setItem('showUdgifter', showUdgifter);
        localStorage.setItem('showSøjlediagram', showSøjlediagram);

        alert('Settings saved!');
    };

    return (
    <Box sx={{ padding: 4 }}>
        <Typography variant="h7" gutterBottom>
        Vælg hvilke ting der skal vises på forsiden:
        </Typography>

        {/* Gentag denne boks for hver feature der skal tilføjes, label er tekst ved siden af check boks*/}
        <Box>
        <FormControlLabel
         control={<Checkbox checked={showPieChart} onChange={(e) => setShowPieChart(e.target.checked)} />}
         label="Vis PieChart" />  
        </Box>

        <Box>
        <FormControlLabel
         control={<Checkbox checked={showBudget} onChange={(e) => setShowBudget(e.target.checked)} />}
         label="Vis Budget" />  
        </Box>

        <Box>
        <FormControlLabel
         control={<Checkbox checked={showIndtægter} onChange={(e) => setShowIndtægter(e.target.checked)} />}
         label="Vis Indtægter" />  
        </Box>

        <Box>
        <FormControlLabel
         control={<Checkbox checked={showUdgifter} onChange={(e) => setShowUdgifter(e.target.checked)} />}
         label="Vis Udgifter" />  
        </Box>

        <Box>
        <FormControlLabel
         control={<Checkbox checked={showSøjlediagram} onChange={(e) => setShowSøjlediagram(e.target.checked)} />}
         label="Vis Søjlediagram" />  
        </Box>

        <Box>
            <Button variant="contained" color="primary" onClick={handleSaveSettings}>
                Gem indstillinger
            </Button>
        </Box>
        <Typography variant="h8" gutterBottom>
        Til senere skal ses om der kan gemmes uden en pop up der siger det er gemt 
        </Typography>
    </Box>
    );
} 
export default DesignFrontPage;