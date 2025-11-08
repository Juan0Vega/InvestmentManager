import '../Index.css';
import { useState, useEffect } from 'react';
import { fetchAllFunds } from '../services/fundService';

export default function Funds() {
  const [funds, setFunds] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    const load = async () => {
      setLoading(true);
      setError(null);
      try {
        const data = await fetchAllFunds();
        setFunds(data || []);
      } catch (err) {
        setError('No se pudieron cargar los fondos');
      } finally {
        setLoading(false);
      }
    };
    load();
  }, []);

  return (
    <div className="p-6">
      <div className="flex items-center justify-between mb-6">
        <div>
          <h1 className="text-3xl font-bold">Fondos Disponibles</h1>
        </div>
        <button className="flex items-center justify-center gap-2 min-w-[84px] cursor-pointer rounded-lg h-11 px-5 bg-accent text-white text-sm font-bold leading-normal transition-opacity hover:opacity-90 shadow-sm">
          <span className="truncate">Añadir Fondo</span>
        </button>
      </div>

      <div className="overflow-x-auto rounded-lg shadow">
        <table className="min-w-full bg-white">
          <thead className="border-b border-slate-200 bg-white">
            <tr>
              <th className="px-6 py-4 text-left text-xs font-bold text-gray-500 uppercase tracking-wider">Nombre</th>
              <th className="px-6 py-4 text-left text-xs font-bold text-gray-500 uppercase tracking-wider">Inversión Minima</th>
              <th className="px-6 py-4 text-left text-xs font-bold text-gray-500 uppercase tracking-wider">Categoria</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-200">
            {loading && (
              <tr>
                <td colSpan={3} className="px-6 py-4 text-sm text-gray-500">Cargando fondos...</td>
              </tr>
            )}
            {error && (
              <tr>
                <td colSpan={3} className="px-6 py-4 text-sm text-red-500">{error}</td>
              </tr>
            )}
            {!loading && !error && funds.length === 0 && (
              <tr>
                <td colSpan={3} className="px-6 py-4 text-sm text-gray-500">No hay fondos disponibles</td>
              </tr>
            )}
            {!loading && !error && funds.map((f) => (
              <tr key={f.fundId} className="hover:bg-gray-50">
                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                  {f.name}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm">
                  <span className="inline-flex items-center px-2 py-0.5 text-blue-700 font-semibold bg-accent/10 rounded-md">
                    ${(f.minAmount ?? 0).toLocaleString()}
                  </span>
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                  {f.category}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
