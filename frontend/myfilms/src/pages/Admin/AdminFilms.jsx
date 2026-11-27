import API from '../../services/filmesApi';
import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { toast } from 'react-toastify';
import { Box, Button, Card, Image } from "@chakra-ui/react"



export default function AdminFilms(){

    const [filmes, setFilmes] = useState([])

    useEffect(() => {
    async function getFilms() {
    try{
        const data = await API()
        setFilmes(data)
    }catch(error){
        return;
    }}
    getFilms()
},[])

return(
    <>
        {filmes.map(filmes => (
            <Card.Root key={filmes.id} width={400} overflow={'hidden'}>
                <Box>
                <Image src={filmes.thumbnailUrl}></Image>
                <Card.Body>
                <Card.Title>{filmes.title}</Card.Title>
                <Card.Description>{filmes.description}</Card.Description>
                <Card.Footer>
                    <Button colorPalette={'yellow'} variant={'outline'}>Editar</Button>
                    <Button colorPalette={'red'}>Deletar</Button>
                </Card.Footer>
                </Card.Body>
                </Box>
            </Card.Root>
        ))}
    </>
)


}