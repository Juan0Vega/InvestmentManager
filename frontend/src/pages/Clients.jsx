import '../Index.css';
import { useState, useEffect } from 'react';
import ClientModal from '../components/ClientModal';
import AddClientModal from '../components/AddClientModal';
import { fetchClients } from '../services/clientService';

export default function Clients() {
  const [clients, setClients] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isAddClientOpen, setIsAddClientOpen] = useState(false);
  const [selectedClient, setSelectedClient] = useState(null);

  useEffect(() => {
    const loadClients = async () => {
      try {
        const clientsData = await fetchClients();
        setClients(clientsData);
      } catch (error) {
        console.error('Error loading clients:', error);
      }
    };

    loadClients();
  }, []);

  const handleOpenModal = (client) => {
    setSelectedClient(client);
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
    setSelectedClient(null);
  };

  const handleOpenAddClient = () => setIsAddClientOpen(true);
  const handleCloseAddClient = () => setIsAddClientOpen(false);

  const handleClientCreated = async () => {
    
    try {
      const refreshed = await fetchClients();
      setClients(refreshed);
    } catch (e) {
      console.error('Error reloading clients', e);
    }
  };

  return (
    <div className="p-6">
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-3xl font-bold">Clientes</h1>
        <button
          className="flex items-center justify-center gap-2 min-w-[84px] cursor-pointer rounded-lg h-11 px-5 bg-accent text-white text-sm font-bold leading-normal transition-opacity hover:opacity-90 shadow-sm"
          onClick={handleOpenAddClient}
        >
          <span className="truncate">Añadir Cliente</span>
        </button>
      </div>

      <div className="overflow-x-auto rounded-lg shadow">
        <table className="min-w-full bg-white">
          <thead className="border-b border-slate-200 bg-white">
            <tr>
              <th className="px-6 py-4 text-left text-xs font-bold text-gray-500 uppercase tracking-wider">Nombre</th>
              <th className="px-6 py-4 text-left text-xs font-bold text-gray-500 uppercase tracking-wider">Correo Electrónico</th>
              <th className="px-6 py-4 text-left text-xs font-bold text-gray-500 uppercase tracking-wider">Balance Actual</th>
              <th className="relative px-6 py-4">
                <span className="sr-only">Acciones</span>
              </th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-200">
            {clients.map((client) => (
              <tr key={client.clientId} className="hover:bg-gray-50">
                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                  {client.name}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                  {client.email}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm">
                  <span className="inline-flex items-center px-2 py-0.5 text-green-700 font-semibold bg-green-50 rounded-md">
                    ${client.currentBalance ? client.currentBalance.toLocaleString() : 'N/A'}
                  </span>
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-right text-md font-medium">
                  <button 
                    className="cursor-pointer text-accent/70 transform transition-all duration-300 px-3 py-1.5 rounded-md font-semibold hover:bg-accent/10 hover:text-accent hover:scale-102 active:scale-98"
                    onClick={() => handleOpenModal(client)}
                  >
                    Gestionar
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      
      <ClientModal
        isOpen={isModalOpen}
        onClose={handleCloseModal}
        client={selectedClient}
        onClientsRefresh={handleClientCreated} 
      />

      <AddClientModal
        isOpen={isAddClientOpen}
        onClose={handleCloseAddClient}
        onCreated={handleClientCreated}
      />
    </div>
  );
}
