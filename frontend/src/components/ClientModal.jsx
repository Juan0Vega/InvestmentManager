import React, { useEffect, useState } from 'react';
import { fetchTransactionsByClientId, cancelTransaction, createTransaction } from '../services/transactionService';
import { fetchAllFunds } from '../services/fundService';

const ClientModal = ({ isOpen, onClose, client, onClientsRefresh }) => {
  const [transactions, setTransactions] = useState([]);
  const [loadingTx, setLoadingTx] = useState(false);
  const [txError, setTxError] = useState(null);
  const [cancellingIds, setCancellingIds] = useState(new Set());

  // fondos
  const [funds, setFunds] = useState([]);
  const [loadingFunds, setLoadingFunds] = useState(false);
  const [fundsError, setFundsError] = useState(null);
  const [selectedFundId, setSelectedFundId] = useState('');
  // nuevo estado para crear inversión
  const [amount, setAmount] = useState('');
  const [creating, setCreating] = useState(false);
  const [createError, setCreateError] = useState(null);

  const loadTransactions = async () => {
    if (!client) return;
    setLoadingTx(true);
    setTxError(null);
    try {
      const transactionsData = await fetchTransactionsByClientId(client.clientId);
      setTransactions(transactionsData || []);
    } catch (error) {
      console.error('Error loading transactions:', error);
      setTxError('No se pudieron cargar las transacciones');
      setTransactions([]);
    } finally {
      setLoadingTx(false);
    }
  };

  const loadFunds = async () => {
    setLoadingFunds(true);
    setFundsError(null);
    try {
      const data = await fetchAllFunds();
      setFunds(data || []);

      if ((data || []).length > 0) setSelectedFundId(String(data[0].fundId));
    } catch (err) {
      console.error('Error loading funds', err);
      setFunds([]);
      setFundsError('No se pudieron cargar los fondos');
    } finally {
      setLoadingFunds(false);
    }
  };

  useEffect(() => {
    if (!isOpen || !client) return;
    loadTransactions();
    loadFunds();
  }, [isOpen, client]);

  const handleCancelTransaction = async (transactionId) => {
    if (!transactionId || !client) return;

    setTxError(null);
    setCancellingIds(prev => {
      const copy = new Set(prev);
      copy.add(transactionId);
      return copy;
    });

    try {
      await cancelTransaction(client.clientId, transactionId);
  
      await loadTransactions();
      
      if (typeof onClientsRefresh === 'function') {
        await onClientsRefresh();
      }
      onClose && onClose();
    } catch (error) {
      console.error('Cancel failed:', error);
      setTxError('No se pudo cancelar la transacción. Intenta de nuevo.');
    } finally {
      setCancellingIds(prev => {
        const copy = new Set(prev);
        copy.delete(transactionId);
        return copy;
      });
    }
  };

  const handleCreateInvestment = async (e) => {
    e.preventDefault();
    setCreateError(null);

    if (!client) {
      setCreateError('Cliente no seleccionado');
      return;
    }
    if (!selectedFundId) {
      setCreateError('Selecciona un fondo');
      return;
    }
    const monto = Number(amount);
    if (!monto || monto <= 0) {
      setCreateError('Introduce un monto válido');
      return;
    }

    const fund = funds.find(f => String(f.fundId) === String(selectedFundId));
    
    const transactionPayload = {
      transactionId: "",
      clientId: client.clientId,
      fundId: fund ? fund.fundId : Number(selectedFundId),
      fundName: fund ? fund.name : "",
      type: "OPEN",
      amount: monto,
      timestamp: "",
      balanceAfter: 0
    };

    setCreating(true);
    try {
      await createTransaction(transactionPayload);

      await loadTransactions();
    
      if (typeof onClientsRefresh === 'function') {
        await onClientsRefresh();
      }
      onClose && onClose();
      setAmount('');
    } catch (err) {
      console.error('Create investment failed', err);
      setCreateError('No se pudo crear la inversión. Intenta de nuevo.');
    } finally {
      setCreating(false);
    }
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 flex items-start md:items-center justify-center bg-black/50 z-50 p-4 overflow-auto">
      <div className="relative bg-white rounded-lg shadow-lg w-full max-w-6xl max-h-[90vh] overflow-y-auto p-6">
        <button
          aria-label="Cerrar"
          className="absolute top-4 right-4 text-gray-600 hover:text-gray-800"
          onClick={onClose}
        >
          X
        </button>

        <h2 className="text-2xl font-semibold mb-4">Gestión del cliente {client?.name ?? ''}</h2>

     
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
      
          <div className="md:col-span-2 bg-white border border-gray-200 rounded-lg shadow-sm p-6">
            <h3 className="text-lg font-medium text-gray-800 mb-3">Resumen del Cliente</h3>
            <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
              <div>
                <p className="text-sm text-gray-500">Nombre</p>
                <p className="font-medium">{client?.name ?? '—'}</p>
              </div>
              <div>
                <p className="text-sm text-gray-500">Email</p>
                <p className="font-medium">{client?.email ?? '—'}</p>
              </div>
              <div>
                <p className="text-sm text-gray-500">Balance Total</p>
                <p className="font-medium text-green-700">${(client?.currentBalance ?? 0).toLocaleString()}</p>
              </div>
            </div>
          </div>

          <div className="bg-white border border-gray-200 rounded-lg shadow-sm p-4 overflow-auto">
            <h4 className="text-md font-semibold mb-3">Inversiones Activas</h4>

            {loadingTx && <p className="text-sm text-gray-500">Cargando...</p>}
            {txError && <p className="text-sm text-red-500">{txError}</p>}

            {!loadingTx && !txError && transactions.length === 0 && (
              <p className="text-sm text-gray-500">No hay inversiones activas</p>
            )}

            {!loadingTx && transactions.length > 0 && (
              <div className="overflow-x-auto">
                <table className="min-w-full text-sm">
                  <thead>
                    <tr className="bg-gray-100">
                      <th className="px-3 py-2 text-left">Fondo</th>
                      <th className="px-3 py-2 text-left">Tipo</th>
                      <th className="px-3 py-2 text-right">Monto</th>
                      <th className="px-3 py-2 text-left">Fecha</th>
                      <th className="px-3 py-2 text-center">Acción</th>
                    </tr>
                  </thead>
                  <tbody>
                    {transactions.map(tx => (
                      <tr key={tx.transactionId} className="odd:bg-white even:bg-gray-50">
                        <td className="px-3 py-2">{tx.fundName ?? '—'}</td>
                        <td className="px-3 py-2">{tx.type ?? '—'}</td>
                        <td className="px-3 py-2 text-right">${(tx.amount ?? 0).toLocaleString()}</td>
                        <td className="px-3 py-2">{tx.timestamp ? new Date(tx.timestamp).toLocaleString() : '—'}</td>
                        <td className="px-3 py-2 text-center">
                          {tx.type === 'OPEN' ? (
                            <button
                              type="button"
                              onClick={() => handleCancelTransaction(tx.transactionId)}
                              className="cursor-pointer inline-flex items-center justify-center w-8 h-8 text-red-600 hover:text-red-800 bg-red-50 rounded-full"
                              title="Cancelar"
                              aria-label={`Cancelar transacción ${tx.transactionId}`}
                              disabled={cancellingIds.has(tx.transactionId)}
                            >
                              {cancellingIds.has(tx.transactionId) ? (
                                <svg className="w-4 h-4 animate-spin" viewBox="0 0 24 24"><circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" fill="none"></circle><path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8v4a4 4 0 00-4 4H4z"></path></svg>
                              ) : (
                                'X'
                              )}
                            </button>
                          ) : (
                            <span className="text-sm text-gray-400">—</span>
                          )}
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )}
          </div>

          <div className="bg-white border border-gray-200 rounded-lg shadow-sm p-4">
            <h4 className="text-md font-semibold mb-3">Nueva Inversión</h4>
            <form className="space-y-3" onSubmit={handleCreateInvestment}>
              <div>
                <label className="block text-sm text-gray-600 mb-1">Monto</label>
                <input
                  type="number"
                  className="w-full rounded-md border-gray-300 p-2"
                  placeholder="$0.00"
                  value={amount}
                  onChange={(e) => setAmount(e.target.value)}
                />
              </div>
              <div>
                <label className="block text-sm text-gray-600 mb-1">Fondo</label>
                {loadingFunds ? (
                  <div className="text-sm text-gray-500">Cargando fondos...</div>
                ) : fundsError ? (
                  <div className="text-sm text-red-500">{fundsError}</div>
                ) : (
                  <select
                    className="w-full rounded-md border-gray-300 p-2"
                    value={selectedFundId}
                    onChange={(e) => setSelectedFundId(e.target.value)}
                  >
                    <option value="">Seleccionar fondo...</option>
                    {funds.map(f => (
                      <option key={f.fundId} value={f.fundId}>
                        {f.name} {f.minAmount ? `(mín $${f.minAmount})` : ''}
                      </option>
                    ))}
                  </select>
                )}
              </div>

              {createError && <p className="text-sm text-red-500">{createError}</p>}

              <button
                type="submit"
                className="w-full bg-accent text-white py-2 rounded-md hover:bg-accent/90 disabled:opacity-60"
                disabled={creating}
              >
                {creating ? 'Creando...' : 'Crear Inversión'}
              </button>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ClientModal;

