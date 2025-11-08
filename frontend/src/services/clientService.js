import { ClientDto } from '../dtos/clientDto';

const API_URL = 'https://qd0vkbuo9l.execute-api.us-east-1.amazonaws.com/api/Clients/GetAllClients';
const CLIENTS_API = 'https://qd0vkbuo9l.execute-api.us-east-1.amazonaws.com/api/Clients';

export const fetchClients = async () => {
  try {
    const response = await fetch(API_URL, {
      method: "GET",
      headers: { "Content-Type": "application/json" },
    });
    if (!response.ok) {
      throw new Error('Error al obtener los clientes');
    }
    const data = await response.json();
    return data.map(client => new ClientDto(
      client.clientId,
      client.name,
      client.email,
      client.phone,
      client.preferredNotification,
      client.currentBalance
    ));
  } catch (error) {
    console.error('Error fetching clients:', error);
    throw error; 
  }
};

export const createClient = async (payload) => {
  try {
    const body = JSON.stringify({
      clientId: payload.clientId ?? 0,
      name: payload.name,
      email: payload.email,
      phone: payload.phone,
      preferredNotification: payload.preferredNotification,
      currentBalance: payload.currentBalance ?? 0
    });

    const resp = await fetch(`${CLIENTS_API}/CreateClient`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body,
    });

    const text = await resp.text();
    if (!resp.ok) {
      throw new Error(`CreateClient failed: ${resp.status} ${text}`);
    }
    return text ? JSON.parse(text) : null;
  } catch (err) {
    console.error('createClient error', err);
    throw err;
  }
};
