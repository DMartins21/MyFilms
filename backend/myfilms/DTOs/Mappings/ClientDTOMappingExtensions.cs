using myfilms.Models;

namespace myfilms.DTOs.Mappings;

public static class ClientDTOMappingExtensions
{
    public static ClientDTO? ToClientDTO(this Client client)
    {
        if(client is null)
            return null;
		
        var clientDto = new ClientDTO()
        {
            Name = client.Name,
            LastName = client.LastName,
            ProfilePictureUrl = client.ProfilePictureUrl,
            BirthDate = client.BirthDate,
        };
        
        return clientDto;
    }
    
    public static Client? ToClient(this ClientDTO clientDto)
    {
        if(clientDto is null)
            return null;
		
        var client = new Client()
        {
            Name = clientDto.Name,
            LastName = clientDto.LastName,
            ProfilePictureUrl = clientDto.ProfilePictureUrl,
            BirthDate = clientDto.BirthDate,
            UserId = null
        };
        
        return client;
    }
}