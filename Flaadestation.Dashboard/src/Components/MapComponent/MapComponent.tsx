import "./MapComponent.css";
import { useEffect, useState } from "react";
import { APIProvider, Map } from "@vis.gl/react-google-maps";
import { JobModel } from "../../Models/Job";
import { jobService } from "../../Services/JobService";
import { dawaService } from "../../Services/DawaService";
import { JobMarkerComponent } from "../JobMarkerComponent/JobMarkerComponent";
import type { BaseModel } from "../../Models/Base";
import { baseService } from "../../Services/BaseService";
import { BaseMarkerComponent } from "../BaseMarkerComponent/BaseMarkerComponent";
import { MapVehicleModal, type VehicleModalProps } from "./MapVehicleModal";
import { ItemType } from "../../Constants/ItemType";
import { MapEmployeeModal, type EmployeeModalProps } from "./MapEmployeeModal";
import { MapToolModal, type ToolModalProps } from "./MapToolModal";
import { MapMachineryModal, type MachineryModalProps } from "./MapMachineryModal";
import { vehicleService } from "../../Services/VehicleService";
import { toolService } from "../../Services/ToolService";
import { machineryService } from "../../Services/MachineryService";
import { employeeService } from "../../Services/EmployeeService";
import { useAuth } from "../../Auth/AuthContext";
import MapModal, { type JobFormData, type BaseFormData } from "./MapModal";

type MarkerFilter = "all" | "jobs" | "bases";

export default function GoogleMap() {
  const { user } = useAuth();
  const [jobs, setJobs] = useState<JobModel[]>([]);
  const [bases, setBases] = useState<BaseModel[]>([]);
  const [vehicles, setVehicles] = useState<any[]>([]);
  const [tools, setTools] = useState<any[]>([]);
  const [machinery, setMachinery] = useState<any[]>([]);
  const [employees, setEmployees] = useState<any[]>([]);
  const [activeMarkerId, setActiveMarkerId] = useState<string | null>(null);
  const [modalOpen, setModalOpen] = useState(false);
  const [modalType, setModalType] = useState<ItemType | null>(null);
  const [selectedJob, setSelectedJob] = useState<JobModel | null>(null);
  const [selectedBase, setSelectedBase] = useState<BaseModel | null>(null);
  const [markerFilter, setMarkerFilter] = useState<MarkerFilter>('all');
  const [searchTerm, setSearchTerm] = useState('');
  const [creationModalOpen, setCreationModalOpen] = useState(false);
  const apiKey = import.meta.env.VITE_GOOGLE_MAPS_API_KEY as string;

  useEffect(() => {
    loadAllData();
  }, []);

  const loadAllData = async () => {
    try {
      const jobModels = await jobService.getJobsByCompany();
      await dawaService.fillAddressesOnJobs(jobModels);
      setJobs(jobModels);

      const baseModels = await baseService.getBasesByCompany();
      await dawaService.fillAddressesOnBases(baseModels);
      setBases(baseModels);

      const allVehicles = await vehicleService.getAllVehicles();
      setVehicles(allVehicles);

      const allTools = await toolService.getAllTools();
      setTools(allTools);

      const allMachinery = await machineryService.getAllMachinery();
      setMachinery(allMachinery);

      const allEmployees = await employeeService.getAllEmployees();
      setEmployees(allEmployees);
    } catch (error) {
      console.error('Error loading data:', error);
    }
  };

  const filteredJobs = useMemo(() => {
    if (!searchTerm.trim()) return jobs;

    const searchLower = searchTerm.toLowerCase();

    return jobs.filter((job: JobModel) => {
      const now = new Date();
      const jobStart = new Date(job.data.scheduledStart);
      const jobEnd = new Date(job.data.scheduledEnd);
      const isActiveJob = now >= jobStart && now <= jobEnd;

      if (!isActiveJob) return false;

      const matchesJob = job.data.title.toLowerCase().includes(searchLower) ||
        job.data.description?.toLowerCase().includes(searchLower);

      const matchesEmployees = job.data.storage.employees.some((emp: any) =>
        emp.firstName.toLowerCase().includes(searchLower) ||
        emp.lastName.toLowerCase().includes(searchLower)
      ) || job.data.storage.defaultEmployees.some((emp: any) =>
        emp.firstName.toLowerCase().includes(searchLower) ||
        emp.lastName.toLowerCase().includes(searchLower)
      );

      const matchesVehicles = job.data.storage.vehicles.some((vehicle: any) =>
        vehicle.model.toLowerCase().includes(searchLower) ||
        vehicle.licensePlate?.toLowerCase().includes(searchLower)
      ) || job.data.storage.defaultVehicles.some((vehicle: any) =>
        vehicle.model.toLowerCase().includes(searchLower) ||
        vehicle.licensePlate?.toLowerCase().includes(searchLower)
      );

      const matchesTools = job.data.storage.tools.some((tool: any) =>
        tool.name.toLowerCase().includes(searchLower)
      ) || job.data.storage.defaultTools.some((tool: any) =>
        tool.name.toLowerCase().includes(searchLower)
      );

      const matchesMachinery = job.data.storage.machines.some((machine: any) =>
        machine.name.toLowerCase().includes(searchLower)
      ) || job.data.storage.defaultMachines.some((machine: any) =>
        machine.name.toLowerCase().includes(searchLower)
      );

      return matchesJob || matchesEmployees || matchesVehicles || matchesTools || matchesMachinery;
    });
  }, [jobs, searchTerm]);

  const filteredBases = useMemo(() => {
    if (!searchTerm.trim()) return bases;

    const searchLower = searchTerm.toLowerCase();

    return bases.filter((base: BaseModel) => {
      const matchesBase = base.data.name.toLowerCase().includes(searchLower);

      const matchesVehicles = vehicles.some((vehicle: any) => {
        const vehicleMatches = vehicle.model.toLowerCase().includes(searchLower) ||
          vehicle.licensePlate?.toLowerCase().includes(searchLower);
        if (!vehicleMatches) return false;

        const belongsToBase = vehicle.defaultStorage?.relevantId === base.data.baseId;
        if (!belongsToBase) return false;

        const hasActiveStorageItem = vehicle.storageItems?.some((item: any) => {
          const now = new Date();
          const itemStart = new Date(item.scheduledStart);
          const itemEnd = new Date(item.scheduledEnd);
          return now >= itemStart && now <= itemEnd;
        });

        return !hasActiveStorageItem;
      });

      const matchesEmployees = employees.some((employee: any) => {
        const firstName = employee.data?.firstName || employee.firstName;
        const lastName = employee.data?.lastName || employee.lastName;
        const employeeMatches = firstName.toLowerCase().includes(searchLower) ||
          lastName.toLowerCase().includes(searchLower);
        if (!employeeMatches) return false;

        const belongsToBase = employee.data?.defaultStorage?.relevantId === base.data.baseId;
        if (!belongsToBase) return false;

        const hasActiveStorageItem = employee.currentAssignment !== null;
        return !hasActiveStorageItem;
      }) || base.data.storage.defaultEmployees.some((emp: any) => {
        const employeeMatches = emp.firstName.toLowerCase().includes(searchLower) ||
          emp.lastName.toLowerCase().includes(searchLower);
        if (!employeeMatches) return false;

        const hasActiveJob = jobs.some((job: JobModel) => {
          const now = new Date();
          const jobStart = new Date(job.data.scheduledStart);
          const jobEnd = new Date(job.data.scheduledEnd);
          const isActiveJob = now >= jobStart && now <= jobEnd;

          if (!isActiveJob) return false;

          return job.data.storage.defaultEmployees.some((jobEmp: any) =>
            jobEmp.firstName === emp.firstName && jobEmp.lastName === emp.lastName
          );
        });

        return !hasActiveJob;
      });

      const matchesTools = tools.some((tool: any) => {
        const toolMatches = tool.name.toLowerCase().includes(searchLower);
        if (!toolMatches) return false;

        const belongsToBase = tool.data?.defaultStorage?.relevantId === base.data.baseId;
        if (!belongsToBase) return false;

        const hasActiveStorageItem = tool.currentAssignment !== null;
        return !hasActiveStorageItem;
      }) || base.data.storage.defaultTools.some((tool: any) => {
        const toolMatches = tool.name.toLowerCase().includes(searchLower);
        if (!toolMatches) return false;

        const hasActiveJob = jobs.some((job: JobModel) => {
          const now = new Date();
          const jobStart = new Date(job.data.scheduledStart);
          const jobEnd = new Date(job.data.scheduledEnd);
          const isActiveJob = now >= jobStart && now <= jobEnd;

          if (!isActiveJob) return false;

          return job.data.storage.defaultTools.some((jobTool: any) => jobTool.name === tool.name);
        });

        return !hasActiveJob;
      });

      const matchesMachinery = machinery.some((machine: any) => {
        const machineMatches = machine.name.toLowerCase().includes(searchLower);
        if (!machineMatches) return false;

        const belongsToBase = machine.data?.defaultStorage?.relevantId === base.data.baseId;
        if (!belongsToBase) return false;

        const hasActiveStorageItem = machine.currentAssignment !== null;
        return !hasActiveStorageItem;
      }) || base.data.storage.defaultMachines.some((machine: any) => {
        const machineMatches = machine.name.toLowerCase().includes(searchLower);
        if (!machineMatches) return false;

        const hasActiveJob = jobs.some((job: JobModel) => {
          const now = new Date();
          const jobStart = new Date(job.data.scheduledStart);
          const jobEnd = new Date(job.data.scheduledEnd);
          const isActiveJob = now >= jobStart && now <= jobEnd;

          if (!isActiveJob) return false;

          return job.data.storage.defaultMachines.some((jobMachine: any) => jobMachine.name === machine.name);
        });

        return !hasActiveJob;
      });

      return matchesBase || matchesEmployees || matchesVehicles || matchesTools || matchesMachinery;
    });
  }, [bases, vehicles, tools, machinery, employees, jobs, searchTerm]);

  const handleCreateJob = async (formData: JobFormData) => {
    try {
      if (!user?.companyId) {
        throw new Error('No company ID available');
      }
      const newJob = await jobService.createJob(formData, user.companyId);
      await loadAllData(); 
    } catch (error) {
      console.error('Error creating job:', error);
      throw error;
    }
  };

  const handleCreateBase = async (formData: BaseFormData) => {
    try {
      if (!user?.companyId) {
        throw new Error('No company ID available');
      }
      const newBase = await baseService.createBase(formData, user.companyId);
      await loadAllData(); 
    } catch (error) {
      console.error('Error creating base:', error);
      throw error;
    }
  };

  const denmarkBounds = {
    north: 57.85,
    south: 54.26,
    west: 7.9,
    east: 15.26,
  };

  const shouldShowJobs = markerFilter === "all" || markerFilter === "jobs";
  const shouldShowBases = markerFilter === "all" || markerFilter === "bases";

  const selectJob = (job: JobModel | null) => {
    setSelectedJob(job);
    setSelectedBase(null);
  };

  const selectBase = (base: BaseModel | null) => {
    setSelectedBase(base);
    setSelectedJob(null);
  };

  const openModal = (itemType: ItemType) => {
    setModalType(itemType);
    setModalOpen(true);
  };

  const closeModal = () => {
    setModalType(null);
    setModalOpen(false);
  };

  function mapToVehicleModalProps(): VehicleModalProps | null {
    if (selectedJob) {
      return {
        vehicles: selectedJob.data.storage.vehicles,
        defaultVehicles: selectedJob.data.storage.defaultVehicles,
        onClose: closeModal,
      };
    } else if (selectedBase) {
      return {
        vehicles: selectedBase.data.storage.vehicles,
        defaultVehicles: selectedBase.data.storage.defaultVehicles,
        onClose: closeModal,
      };
    }

    return null;
  }

  function mapToEmployeeModalProps(): EmployeeModalProps | null {
    if (selectedJob) {
      return {
        employees: selectedJob.data.storage.employees,
        defaultEmployees: selectedJob.data.storage.defaultEmployees,
        onClose: closeModal,
      };
    } else if (selectedBase) {
      return {
        employees: selectedBase.data.storage.employees,
        defaultEmployees: selectedBase.data.storage.defaultEmployees,
        onClose: closeModal,
      };
    }

    return null;
  }

  function mapToToolModalProps(): ToolModalProps | null {
    if (selectedJob) {
      return {
        tools: selectedJob.data.storage.tools,
        defaultTools: selectedJob.data.storage.defaultTools,
        onClose: closeModal,
      };
    } else if (selectedBase) {
      return {
        tools: selectedBase.data.storage.tools,
        defaultTools: selectedBase.data.storage.defaultTools,
        onClose: closeModal,
      };
    }

    return null;
  }

  function mapToMachineryModalProps(): MachineryModalProps | null {
    if (selectedJob) {
      return {
        machines: selectedJob.data.storage.machines,
        defaultMachines: selectedJob.data.storage.defaultMachines,
        onClose: closeModal,
      };
    } else if (selectedBase) {
      return {
        machines: selectedBase.data.storage.machines,
        defaultMachines: selectedBase.data.storage.defaultMachines,
        onClose: closeModal,
      };
    }

    return null;
  }

  return (
    <div className="map-container">
      <APIProvider apiKey={apiKey}>
        <Map
          restriction={{
            latLngBounds: denmarkBounds,
            strictBounds: false,
          }}
          mapId={"1"}
          defaultCenter={{ lat: 56.26392, lng: 9.501785 }}
          defaultZoom={6}
          style={{
            height: "100%",
            width: "100%",
          }}
        >
          {shouldShowJobs && filteredJobs.map((job) => (
              <JobMarkerComponent
                key={job.data.jobId}
                job={job}
                activeMarkerId={activeMarkerId}
                setMarkerJobId={setActiveMarkerId}
                setSelectedJob={selectJob}
                openModal={openModal}
              />
            ))}

          {shouldShowBases && filteredBases.map((base) => (
              <BaseMarkerComponent
                key={base.data.baseId}
                base={base}
                activeMarkerId={activeMarkerId}
                setMarkerBaseId={setActiveMarkerId}
                setSelectedBase={selectBase}
                openModal={openModal}
              />
            ))}
        </Map>

        <div className="map-search-controls">
          <div className="search-input-container">
            <div className="input-group">
              <span className="input-group-text">
                <i className="bi bi-search"></i>
              </span>
              <input
                type="text"
                className="form-control"
                placeholder="Søg efter medarbejdere, køretøjer, værktøj..."
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
        </div>

        <div className="map-filter-controls">
          <div className="filter-buttons">
            <button
              className="filter-btn filter-btn-create"
              onClick={() => setCreationModalOpen(true)}
              title="Opret ny opgave eller base"
              style={{
                backgroundColor: '#28a745',
                marginBottom: '12px'
              }}
            >
              <i className="bi bi-plus"></i>
            </button>

            <button
              className={`filter-btn filter-btn-job ${markerFilter === 'jobs' ? 'active' : ''}`}
              onClick={() => setMarkerFilter(markerFilter === 'jobs' ? 'all' : 'jobs')}
              title="Vis opgaver"
            >
              <i className="bi bi-tools"></i>
            </button>

            <button
              className={`filter-btn filter-btn-base ${
                markerFilter === "bases" ? "active" : ""
              }`}
              onClick={() =>
                setMarkerFilter(markerFilter === "bases" ? "all" : "bases")
              }
              title="Vis baser"
            >
              <i className="bi bi-house-fill"></i>
            </button>
          </div>
        </div>
        {modalOpen &&
          modalType === ItemType.Vehicle &&
          (() => {
            const props = mapToVehicleModalProps();
            return props ? <MapVehicleModal {...props} /> : null;
          })()}

          {modalOpen &&
          modalType === ItemType.Employee &&
          (() => {
            const props = mapToEmployeeModalProps();
            return props ? <MapEmployeeModal {...props} /> : null;
          })()}

          {modalOpen &&
          modalType === ItemType.Tool &&
          (() => {
            const props = mapToToolModalProps();
            return props ? <MapToolModal {...props} /> : null;
          })()}

          {modalOpen &&
          modalType === ItemType.Mahinery &&
          (() => {
            const props = mapToMachineryModalProps();
            return props ? <MapMachineryModal {...props} /> : null;
          })()}
      </APIProvider>

      <MapModal
        isOpen={creationModalOpen}
        onClose={() => setCreationModalOpen(false)}
        onSaveJob={handleCreateJob}
        onSaveBase={handleCreateBase}
        title="Opret ny opgave eller base"
      />
    </div>
  );
}
