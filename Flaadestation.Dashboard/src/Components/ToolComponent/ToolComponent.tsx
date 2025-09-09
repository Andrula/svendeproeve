import { useState, useEffect, useMemo } from 'react';
import { ToolModel } from '../../Models/Tool';
import { 
  toolService, 
  HttpError
} from '../../Services/ToolService';
import ToolModal, { type ToolFormData } from './ToolModal';
import { useAuth } from '../../Auth/AuthContext';

type SortField = 'name' | 'defaultStorage' | 'availability' | 'currentAssignment';
type SortDirection = 'asc' | 'desc';

export default function ToolComponent() {
  const { user } = useAuth();
  const [tools, setTools] = useState<ToolModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [modalOpen, setModalOpen] = useState(false);
  const [selectedTool, setSelectedTool] = useState<ToolModel | null>(null);

  const [searchTerm, setSearchTerm] = useState('');
  const [sortField, setSortField] = useState<SortField>('name');
  const [sortDirection, setSortDirection] = useState<SortDirection>('asc');

  useEffect(() => {
    loadTools();
  }, []);

  const filteredAndSortedTools = useMemo(() => {
    let filtered = tools;

    if (searchTerm.trim()) {
      const searchLower = searchTerm.toLowerCase();
      filtered = tools.filter(item => {
        return (
          item.name.toLowerCase().includes(searchLower) ||
          item.note?.toLowerCase().includes(searchLower) ||
          item.data.defaultStorage?.name.toLowerCase().includes(searchLower) ||
          item.currentAssignment?.note.toLowerCase().includes(searchLower) ||
          item.nextAssignment?.note.toLowerCase().includes(searchLower)
        );
      });
    }

    const sorted = [...filtered].sort((a, b) => {
      let aValue: string | number;
      let bValue: string | number;

      switch (sortField) {
        case 'name':
          aValue = a.name.toLowerCase();
          bValue = b.name.toLowerCase();
          break;
        case 'defaultStorage':
          aValue = a.data.defaultStorage?.name.toLowerCase() || '';
          bValue = b.data.defaultStorage?.name.toLowerCase() || '';
          break;
        case 'availability':
          aValue = a.isAvailable ? 1 : 0;
          bValue = b.isAvailable ? 1 : 0;
          break;
        case 'currentAssignment':
          aValue = a.currentAssignment ? 1 : 0;
          bValue = b.currentAssignment ? 1 : 0;
          break;
        default:
          aValue = a.name.toLowerCase();
          bValue = b.name.toLowerCase();
      }

      if (sortDirection === 'asc') {
        return aValue > bValue ? 1 : aValue < bValue ? -1 : 0;
      } 
      else {
        return aValue < bValue ? 1 : aValue > bValue ? -1 : 0;
      }
    });

    return sorted;
  }, [tools, searchTerm, sortField, sortDirection]);

  const handleSort = (field: SortField) => {
    if (sortField === field) {
      setSortDirection(sortDirection === 'asc' ? 'desc' : 'asc');
    } else {
      setSortField(field);
      setSortDirection('asc');
    }
  };

  const getSortIcon = (field: SortField) => {
    if (sortField !== field) {
      return <i className="bi bi-arrow-down-up text-muted"></i>;
    }
    return sortDirection === 'asc' 
      ? <i className="bi bi-arrow-up" style={{color: '#fff'}}></i>
      : <i className="bi bi-arrow-down" style={{color: '#fff'}}></i>;
  };

  const loadTools = async () => {
    try {
      setLoading(true);
      setError(null);

      const toolModels = await toolService.getAllTools();
      setTools(toolModels);
    } catch (err) {
      let errorMessage = 'Ukendt fejl';

      if (err instanceof HttpError) {
        if (err.isClientError) {
          errorMessage = `Klient fejl (${err.status}): ${err.message}`;
        } else if (err.isServerError) {
          errorMessage = `Server fejl (${err.status}): ${err.message}`;
        } else {
          errorMessage = err.message;
        }
      } else if (err instanceof Error) {
        errorMessage = err.message;
      }

      setError(`Fejl ved indlæsning af værktøjer: ${errorMessage}`);
      console.error('Fejl ved indlæsning af værktøjer:', err);
    } finally {
      setLoading(false);
    }
  };

  const openCreateModal = () => {
    setSelectedTool(null);
    setModalOpen(true);
  };

  const openEditModal = (tool: ToolModel) => {
    setSelectedTool(tool);
    setModalOpen(true);
  };

  const closeModal = () => {
    setModalOpen(false);
    setSelectedTool(null);
  };

  const handleSaveTool = async (formData: ToolFormData) => {
    if (selectedTool) {
      const updatedTool = await toolService.updateTool(selectedTool.data.itemId, formData);
      setTools(tools.map(tool =>
        tool.data.itemId === updatedTool.data.itemId ? updatedTool : tool
      ));
    } else {
      const newTool = await toolService.createTool(formData, user!.companyId);
      setTools([...tools, newTool]);
    }
  };

  const handleDeleteTool = async (tool: ToolModel) => {
    if (window.confirm(`Er du sikker på, at du vil slette ${tool.name}?`)) {
      try {
        await toolService.deleteTool(tool.data.itemId);
        setTools(tools.filter(t => t.data.itemId !== tool.data.itemId));
      } catch (err) {
        console.error('Error deleting tool:', err);
      }
    }
  };

  if (loading) {
    return (
      <div className="container mt-4">
        <div className="d-flex justify-content-center">
          <div className="spinner-border" role="status">
            <span className="visually-hidden">Indlæser data...</span>
          </div>
          <span className="ms-2">Indlæser værktøjer...</span>
        </div>
      </div>
    );
  }

  return (
    <div className="position-relative">
      <div className="container mt-4">
        <div className="d-flex justify-content-between align-items-center mb-4">
          <h1>Værktøjer</h1>
          <button className="btn btn-primary" onClick={openCreateModal}>
            <i className="bi bi-plus"></i> Tilføj værktøj
          </button>
        </div>

        {error && (
          <div className="alert alert-danger" role="alert">
            {error}
            <button 
              className="btn btn-link p-0 ms-2" 
              onClick={loadTools}
            >
              Prøv igen
            </button>
          </div>
        )}

        {!error && (
          <>
            <div className="row mb-4">
              <div className="col-md-8">
                <div className="input-group">
                  <span className="input-group-text">
                    <i className="bi bi-search"></i>
                  </span>
                  <input
                    type="text"
                    className="form-control"
                    placeholder="Søg efter værktøjsnavn, noter, lager..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                  />
                  {searchTerm && (
                    <button
                      className="btn btn-outline-secondary"
                      type="button"
                      onClick={() => setSearchTerm('')}
                    >
                      <i className="bi bi-x"></i>
                    </button>
                  )}
                </div>
              </div>
              <div className="col-md-4">
                <div className="btn-group w-100" role="group">
                  <button
                    type="button"
                    className={`btn ${sortField === 'name' ? 'btn-primary' : 'btn-outline-primary'}`}
                    onClick={() => handleSort('name')}
                  >
                    Navn {getSortIcon('name')}
                  </button>
                  <button
                    type="button"
                    className={`btn ${sortField === 'availability' ? 'btn-primary' : 'btn-outline-primary'}`}
                    onClick={() => handleSort('availability')}
                  >
                    Status {getSortIcon('availability')}
                  </button>
                  <button
                    type="button"
                    className={`btn ${sortField === 'defaultStorage' ? 'btn-primary' : 'btn-outline-primary'}`}
                    onClick={() => handleSort('defaultStorage')}
                  >
                    Lager {getSortIcon('defaultStorage')}
                  </button>
                </div>
              </div>
            </div>

            <div className="mb-3">
              <small className="text-muted">
                Viser {filteredAndSortedTools.length} af {tools.length} værktøjer
                {searchTerm && (
                  <span> (søgning: "{searchTerm}")</span>
                )}
              </small>
            </div>

            {filteredAndSortedTools.length === 0 ? (
              <div className="alert alert-info">
                {searchTerm ? 
                  `Ingen værktøjer matchede søgningen "${searchTerm}".` : 
                  'Ingen værktøjer fundet.'
                }
              </div>
            ) : (
              <div className="row">
                {filteredAndSortedTools.map((tool) => {
                  const currentAssignment = tool.currentAssignment;
                  const nextAssignment = tool.nextAssignment;

                  return (
                    <div
                      key={tool.data.itemId}
                      className="col-md-6 col-lg-6 mb-3 d-flex"
                    >
                      <div className="card w-100">
                        <div className="card-header bg-dark">
                          <h5 className="card-title d-flex justify-content-between align-items-center">
                            <strong className="text-app-primary">
                              {tool.name}
                            </strong>
                            {tool.isAvailable && (
                              <span className="badge bg-success">Ledig</span>
                            )}
                          </h5>
                        </div>
                        <div className="card-body">
                          <div className="card-text">
                            {tool.data.defaultStorage ? (
                              <p><strong>Standard lager:</strong> {tool.data.defaultStorage.name}</p>
                            ) : (
                              <p className="text-muted fst-italic">ingen standard lager registreret</p>
                            )}
                            {tool.note ? (
                              <>
                                <p><strong>Note:</strong> {tool.note}</p>
                              </>
                            ) : (
                              <>
                                <p className="text-muted fst-italic">
                                  ingen note registreret
                                </p>
                              </>
                            )}
                            {currentAssignment ? (
                              <>
                                <strong>Nuværende opgave:</strong>
                                <div className="bg-light p-2 rounded small">
                                  <small className="text-muted">
                                    {currentAssignment.storage.name}
                                    <br />
                                    {currentAssignment.note}
                                    <br />
                                    {new Date(
                                        currentAssignment.scheduledStart
                                    ).toLocaleDateString("da-DK")}{" "}
                                    -{" "}
                                    {new Date(
                                        currentAssignment.scheduledEnd
                                    ).toLocaleDateString("da-DK")}
                                  </small>
                                </div>
                              </>
                            ) : nextAssignment ? (
                              <>
                                <strong>Næste opgave:</strong>
                                <div className="bg-light p-2 rounded small">
                                  <small className="text-muted">
                                    {nextAssignment.note}
                                    <br />
                                    {new Date(
                                        nextAssignment.scheduledStart
                                    ).toLocaleDateString("da-DK")}{" "}
                                    -{" "}
                                    {new Date(
                                        nextAssignment.scheduledEnd
                                    ).toLocaleDateString("da-DK")}
                                  </small>
                                </div>
                              </>
                            ) : (
                              <>
                                <p className="text-muted fst-italic">
                                  Ikke allokeret til nogle opgaver
                                </p>
                              </>
                            )}
                          </div>
                        </div>
                        <div className="card-footer d-flex justify-content-between">
                          <button 
                            className="btn btn-sm btn-outline-primary me-2"
                            onClick={() => openEditModal(tool)}
                          >
                            <i className="bi bi-pencil"></i> Rediger
                          </button>
                          <button 
                            className="btn btn-sm btn-outline-danger"
                            onClick={() => handleDeleteTool(tool)}
                          >
                            <i className="bi bi-trash"></i> Slet
                          </button>
                        </div>
                      </div>
                    </div>
                  );
                })}
              </div>
            )}
          </>
        )}

        {modalOpen && (
          <ToolModal
            isOpen={modalOpen}
            onClose={closeModal}
            onSave={handleSaveTool}
            tool={selectedTool}
            title={selectedTool ? 'Rediger værktøj' : 'Tilføj nyt værktøj'}
          />
        )}
      </div>
    </div>
  );
}