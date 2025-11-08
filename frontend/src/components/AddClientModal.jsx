import React, { useState } from 'react';
import { createClient } from '../services/clientService';

const AddClientModal = ({ isOpen, onClose, onCreated }) => {
  const [clientId, setClientId] = useState('');
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [phone, setPhone] = useState('');
  const [preferredNotification, setPreferredNotification] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  if (!isOpen) return null;

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);

    if (!name || !email) {
      setError('Campos requeridos');
      return;
    }

    setLoading(true);
    try {
      await createClient({
        clientId,
        name,
        email,
        phone,
        preferredNotification,
        currentBalance: 0
      });
      setClientId('');setName(''); setEmail(''); setPhone(''); setPreferredNotification('');
      onCreated && onCreated();
      onClose && onClose();
    } catch (err) {
      console.error(err);
      setError('No se pudo crear el cliente');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="fixed inset-0 flex items-center justify-center bg-black/50 z-50 p-4">
      <div className="bg-white rounded-lg shadow-lg w-full max-w-md p-6">
        <div className="flex justify-between items-center mb-4">
          <h3 className="text-lg font-semibold">Añadir Cliente</h3>
          <button onClick={onClose} className="text-gray-600">×</button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-3">
          <div>
            <label className="block text-sm text-gray-600 mb-1">Identificación</label>
            <input value={clientId} onChange={e => setClientId(e.target.value)} className="w-full rounded-md border-gray-300 p-2" />
          </div>
          <div>
            <label className="block text-sm text-gray-600 mb-1">Nombre</label>
            <input value={name} onChange={e => setName(e.target.value)} className="w-full rounded-md border-gray-300 p-2" />
          </div>
          <div>
            <label className="block text-sm text-gray-600 mb-1">Email</label>
            <input value={email} onChange={e => setEmail(e.target.value)} className="w-full rounded-md border-gray-300 p-2" />
          </div>
          <div>
            <label className="block text-sm text-gray-600 mb-1">Teléfono</label>
            <input value={phone} onChange={e => setPhone(e.target.value)} className="w-full rounded-md border-gray-300 p-2" />
          </div>
          <div>
            <label className="block text-sm text-gray-600 mb-1">Notificación preferida</label>
            <input value={preferredNotification} onChange={e => setPreferredNotification(e.target.value)} className="w-full rounded-md border-gray-300 p-2" />
          </div>

          {error && <p className="text-sm text-red-500">{error}</p>}

          <button type="submit" disabled={loading} className="w-full bg-accent text-white py-2 rounded-md hover:bg-accent/90 disabled:opacity-60">
            {loading ? 'Creando...' : 'Crear'}
          </button>
        </form>
      </div>
    </div>
  );
};

export default AddClientModal;