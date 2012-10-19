using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Migration
{
    public class MigrationService
    {
        public delegate void ProgressReportDelegate(string message);
        public event ProgressReportDelegate ProgressReport;

        private MigrationSettings _Settings = null;

        public MigrationService(MigrationSettings settings)
        {
            _Settings = settings;
        }

        public void ReIndex()
        {
            wl("Starting Re-Indexing at " + DateTime.UtcNow.ToString());
            wl("=====================================================");

            if (_Settings == null)
            {
                wl("Settings file can not be null");
                return;
            }
            DumpReIndexSettings();

            IMigrator migrator = new Migrators.Reindex.Migrator(); 
            
            if (migrator != null)
            {
                migrator.ProgressReport += new ProgressReportDelegate(migrator_ProgressReport);
                migrator.Migrate(_Settings);
            }

            wl("Ending Re-Indexing at " + DateTime.UtcNow.ToString());
            wl("--EXIT--");
        }

        public void StartMigration()
        {
            if (_Settings.SourceType == MigrationSourceType.ReIndexOnly)
            {
                ReIndex();
                return;
            }

            wl("Starting Migration at " + DateTime.UtcNow.ToString());
            wl("=====================================================");
            
            if (_Settings == null)
            {
                wl("Settings file can not be null");
                return;
            }
            DumpSettings();

            IMigrator migrator = null;
            switch (_Settings.SourceType)
            {
                case MigrationSourceType.MerchantTribe:
                    migrator = new Migrators.MerchantTribe.Migrator();
                    break;
                case MigrationSourceType.BV5:
                    migrator = new Migrators.BV5.Migrator();
                    break;
                case MigrationSourceType.BVC2004:
                    migrator = new Migrators.BV2004.Migrator();
                    break;
            }

            if (migrator != null)
            {
                migrator.ProgressReport += new ProgressReportDelegate(migrator_ProgressReport);
                migrator.Migrate(_Settings);
            }

            wl("Ending Migration at " + DateTime.UtcNow.ToString());
            wl("--EXIT--");
        }

        void migrator_ProgressReport(string message)
        {
            wl(message);
        }

        private void DumpSettings()
        {
            
            wl("--------------------------------------------------");
            wl("");
            wl("Current Settings");
            wl("");
            wl("--------------------------------------------------");
            wl("Import Mode      = " + (_Settings.SourceType == MigrationSourceType.BVC2004 ? "BVC2004" : "BV5"));
            wl("Sending To       = " + _Settings.DestinationServiceRootUrl);
            wl("API Key          = *********");
            wl("");
            wl("Source Server    = " + _Settings.SQLServer);
            wl("Source Database  = " + _Settings.SQLDatabase);
            wl("Source Username  = " + _Settings.SQLUsername);
            wl("Source Password  = *********");
            wl("");
            wl("Image Root: " + _Settings.ImagesRootFolder);
            wl("");
            wl("Import?");
            wl("  Products       = " + _Settings.ImportProducts);
            wl("  Categories     = " + _Settings.ImportCategories);
            wl("  Users          = " + _Settings.ImportUsers);
            wl("  Affiliates     = " + _Settings.ImportAffiliates);
            wl("  Orders         = " + _Settings.ImportOrders);
            wl("  Others         = " + _Settings.ImportOtherSettings);
            wl("");
            wl("Clear?");
            wl("  Products       = " + _Settings.ClearProducts);
            wl("  Categories     = " + _Settings.ClearCategories);
            wl("  Affiliates     = " + _Settings.ClearAffiliates);
            wl("  Orders         = " + _Settings.ClearOrders);
            wl("");
            wl("Single Order #   = " + _Settings.SingleOrderImport);
            wl("Single SKU       = " + _Settings.SingleSkuImport);
            wl("Use Metric Units = " + _Settings.UseMetricUnits);
            wl("");
            wl("--------------------------------------------------");
        }

        private void DumpReIndexSettings()
        {

            wl("--------------------------------------------------");
            wl("");
            wl("Current ReIndex Settings");
            wl("");
            wl("--------------------------------------------------");
            wl("Sending To       = " + _Settings.DestinationServiceRootUrl);
            wl("API Key          = *********");
            wl("--------------------------------------------------");
        }
        private void wl(string message)
        {
            if (this.ProgressReport != null)
            {
                this.ProgressReport(message);
            }
        }
    }
}
