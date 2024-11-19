import { Container, Typography } from "@mui/material";
import React from "react";
import PieChart from "./PieChart";

function Indtægter()
{
   return(
    <Container>
        <Typography variant = "h1" component = "h2">
        Welcome to indtægter 
        </Typography>
        <PieChart/>
    </Container>
   );
}
 
export default Indtægter;