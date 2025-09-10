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

type MarkerFilter = "all" | "jobs" | "bases";

export default function GoogleMap() {
  const [jobs, setJobs] = useState<JobModel[]>([]);
  const [bases, setBases] = useState<BaseModel[]>([]);
  const [activeMarkerId, setActiveMarkerId] = useState<string | null>(null);
  const [markerFilter, setMarkerFilter] = useState<MarkerFilter>("all");
  const [modalOpen, setModalOpen] = useState(false);
  const [modalType, setModalType] = useState<ItemType | null>(null);
  const [selectedJob, setSelectedJob] = useState<JobModel | null>(null);
  const [selectedBase, setSelectedBase] = useState<BaseModel | null>(null);
  const apiKey = import.meta.env.VITE_GOOGLE_MAPS_API_KEY as string;

  useEffect(() => {
    loadJobs();
  }, []);

  const loadJobs = async () => {
    const jobModels = await jobService.getJobsByCompany();
    await dawaService.fillAddressesOnJobs(jobModels);
    setJobs(jobModels);

    const baseModels = await baseService.getBasesByCompany();
    await dawaService.fillAddressesOnBases(baseModels);
    setBases(baseModels);
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
          {shouldShowJobs &&
            jobs.map((job) => (
              <JobMarkerComponent
                key={job.data.jobId}
                job={job}
                activeMarkerId={activeMarkerId}
                setMarkerJobId={setActiveMarkerId}
                setSelectedJob={selectJob}
                openModal={openModal}
              />
            ))}

          {shouldShowBases &&
            bases.map((base) => (
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

        <div className="map-filter-controls">
          <div className="filter-buttons">
            <button
              className={`filter-btn filter-btn-job ${
                markerFilter === "jobs" ? "active" : ""
              }`}
              onClick={() =>
                setMarkerFilter(markerFilter === "jobs" ? "all" : "jobs")
              }
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
    </div>
  );
}
