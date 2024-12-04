import React, { useState, useEffect } from 'react';
import { Dialog, DialogActions, DialogContent, DialogTitle, Button, TextField } from '@mui/material';

export default function EditIncomeDialog({ open, handleClose, handleSave, income }) {
  const [FindtægtId, setFindtægtId] = useState('');
  const [name, setName] = useState('');
  const [price, setPrice] = useState('');
  const [date, setDate] = useState('');
  const [kategoriNavn, setKategoriNavn] = useState('');
  const [kategoriId, setKategoriId] = useState('');

  const token = localStorage.getItem("authToken");

  useEffect(() => {
    if (income) {
      console.log('Income object:', income); // Log income object to verify its structure
      setFindtægtId(income.id); // Set FindtægtId from income
      setName(income.name);
      setPrice(income.price);
      setDate(income.date);
      setKategoriNavn(income.kategoriNavn);
      setKategoriId(income.kategoriId);
    }
  }, [income, open]);

  const handleSubmit = async () => {
    if (name && price && date) {
      const updatedIncome = {
        id: FindtægtId, // Use FindtægtId from state
        Tekst: name,
        Indtægt: price,
        Dato: date,
        KategoriNavn: kategoriNavn,
        KategoriId: kategoriId,
      };
      await handleIndtægt(income);
      handleSave({ id: FindtægtId, name, price, date, kategoriNavn, kategoriId });
      handleClose(); 
    } else {
      alert('Alle felter skal udfyldes!');
    }
  };

 

  const handleIndtægt = async (income) => {
    try {
      const response = await fetch(`http://localhost:5168/api/Findtægt?findid=${FindtægtId}`, { // Use income.id
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        body: JSON.stringify({
            Tekst: income.name,
            Indtægt: income.price,
            Dato: income.date,
            KategoriNavn: income.kategoriNavn,
            KategoriId: income.kategoriId
        }),
      });
      console.log("Income", income);
      console.log("Response", response);
      if (!response.ok) {
        throw new Error('Noget gik galt. Kunne ikke gemme indkomst.');
      }
      
      const result = await response.json();
      console.log('Indkomst gemt:', result);
    } catch (error) {
      console.error('Fejl:', error);
    }
   }

  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>Rediger Indtægt</DialogTitle>
      <DialogContent>
        <TextField
          label="Indtægtsnavn"
          variant="outlined"
          fullWidth
          value={name}
          onChange={(e) => setName(e.target.value)}
          sx={{ marginBottom: 2 }}
        />
        <TextField
          label="Beløb (DKK)"
          variant="outlined"
          fullWidth
          value={price}
          onChange={(e) => setPrice(e.target.value)}
          sx={{ marginBottom: 2 }}
        />
        <TextField
          label="Dato"
          type="date"
          fullWidth
          value={date}
          onChange={(e) => setDate(e.target.value)}
          margin="normal"
          InputLabelProps={{
            shrink: true, 
          }}
        />
<TextField
          label="Kategori"
          type="outlined"
          fullWidth
          value={kategoriNavn}
          onChange={(e) => setKategoriNavn(e.target.value)}
          margin="normal"
          InputLabelProps={{
            shrink: true, 
          }}
        />
<TextField
          label="Kateori ID"
          type="outlined"
          fullWidth
          value={kategoriId}
          onChange={(e) => setKategoriId(e.target.value)}
          margin="normal"
          InputLabelProps={{
            shrink: true, 
          }}
        />
      </DialogContent>
      <DialogActions>
      <Button onClick={handleSubmit} color="primary">
          Gem
        </Button>
        <Button onClick={handleClose} color="secondary">
          Annuller
        </Button>
      </DialogActions>
    </Dialog>
  );
}
