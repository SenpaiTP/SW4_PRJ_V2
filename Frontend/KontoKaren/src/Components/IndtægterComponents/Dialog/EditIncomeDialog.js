import React, { useState, useEffect } from 'react';
import { Dialog, DialogActions, DialogContent, DialogTitle, Button, TextField } from '@mui/material';
import useIndtægterHooks from '../../../Hooks/IndtægterHooks.js';
import { initialRows } from '../Table/TableData';
import { useFetchAllIndtægt } from './FetchAllIndtægt.js';

export default function EditIncomeDialog({ open, handleClose}) {
  const {
    rows, // data i tabellen
    selected, // valgte rækker
    page, // sktuel side
    rowsPerPage, // antal rækker pr. side
    handleClick, // håndtering af klik på rækker
    handleAddRow, // tilføjelse af en ny række
    handleEditRow, // redigering af en række
    handleDeleteRow, // sletning af en række
    handleSave, // gemmer ændringer
    setRows, // opdaterer rækkerne
  } = useIndtægterHooks(initialRows);

  const [income, setIncome] = useState(null);
  const id = income?.id;
  const [row, setRow] = useState([]);

  const [FindtægtId, setFindtægtId] = useState('');
  const [name, setName] = useState('');
  const [price, setPrice] = useState('');
  const [date, setDate] = useState('');
  const [kategoriNavn, setKategoriNavn] = useState('');
  const [kategoriId, setKategoriId] = useState('');
  const token = localStorage.getItem("authToken");

  const fetchIncomes = async () => {

    try {
      const response = await fetch('http://localhost:5168/api/Findtægt', {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });
      if (!response.ok) {
        throw new Error('Kunne ikke hente findtægter.');
      }
  
      const data = await response.json();
      const mappedData = mapData(data);
      console.log("Mapped data: ", mappedData);
      setIncome(mappedData);
      setRows(mappedData); // Opdaterer tabellen med data
    } catch (error) {
      console.error('Fejl:', error.message);
    }
  };

  const handleIndtægt = async (mappedData) => {
    fetchIncomes(); // Debug
    console.log("Income in HandleIndtægt: ", mappedData ); // Debug
    try {
      const response = await fetch('http://localhost:5168/api/Findtægt', {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });
      if (!response.ok) {
        throw new Error('Kunne ikke hente findtægter.');
      }
  
      const data = await response.json();
      const mappedData = mapData(data);
      console.log("Mapped data: ", mappedData);
      setIncome(mappedData);
      setRows(mappedData); // Opdaterer tabellen med data
  
      // After getting the mappedData, make a PUT call for each income (if it's an array)
      mappedData.forEach(async (income) => {
        try {
          const response = await fetch(`http://localhost:5168/api/Findtægt?findid=${income.id}`, { // Use income.FindtægtId
            method: 'PUT',
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${token}`,
            },
            body: JSON.stringify(income),
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
      });
    
    } catch (error) {
      console.error('Fejl:', error.message);
    }

   };

  useIndtægterHooks(() => {
    fetchIncomes();
  }, []);

  const mapData = (data) => {
    return data.map(item => ({
        id: item.findtægtId,
        Tekst: item.tekst,
        Indtægt: item.indtægt,
        Dato: item.dato,
        kategoriNavn: item.kategoriNavn,
        kategoriId: item.kategoriId,
    }));
};

  const handleSubmit = async () => {
    if (name && price && date && kategoriNavn && kategoriId) {
      const updatedIncome = {
        id: FindtægtId, // Use FindtægtId from state
        Tekst: name,
        Indtægt: price,
        Dato: date,
        KategoriNavn: kategoriNavn,
        KategoriId: kategoriId,
      };
      await handleIndtægt(updatedIncome);
      handleSave(updatedIncome);
      handleClose(); 
    } else {
      alert('Alle felter skal udfyldes!');
    }
  };

 

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
